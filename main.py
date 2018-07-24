# Just importing libraries
import cv2
import numpy as np
from collections import Counter
import itertools
from statistics import mode
from pprint import pprint
import sys
import matplotlib.pyplot as plt
import random
import math

radii = []
# Define acceptable error
radiusDelta = 1

# Define acceptable range of dimension sizes, 15-20 usually
minCricleW = 16
minCricleH = 16
minCricleArea = ((minCricleW+minCricleH)/4)*((minCricleW+minCricleH)/4)*3

# Declaring a mcOption class which its objects should each have 9 attributes
class mcOption:
    ID = None
    questionID = None
    optionID = None
    centerX = None
    centerY = None
    circleContour = None
    radius = None
    isChecked = False
    centroidID = None
    
    def __init__(self, ID, questionID, optionID):
    	# Assigning object attribute with recieved parameter values in the bracket
        self.ID = ID
        self.questionID = questionID
        self.optionID = optionID
        
    def initCenters(self, circleContour):
        self.circleContour = circleContour
        self.centerX, self.centerY, self.radius = extractFromCricleContour(circleContour)
    
def extractFromCricleContour(circleContour):
    (x, y, w, h) = cv2.boundingRect(circleContour)	# This boudingRect function in opencv takes in a "contour" datatype
    return [x+w/2, y+h/2, w/2]				# and return 4 values that form a bouding rectangle of the contour/

# Just using opencv functions to grayscale, demoise the image.    
def processImage(image):
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    blurry = cv2.GaussianBlur(gray, (3, 3), 1)
    adapt_thresh = cv2.adaptiveThreshold(blurry, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY_INV, 11, 2)
    return adapt_thresh

def findCircleContours(image):
    processed_image = processImage(image)
    # cv2.fincContours return 3 values, 1st one not needed, "contours" contains all contours in the given image, "hierarchy" is bit special and complicated at first glance https://docs.opencv.org/3.4.0/d9/d8b/tutorial_py_contours_hierarchy.html
    # Basically it describe a "level" relationship between contours and contours
    _, contours, hierarchy = cv2.findContours(processed_image.copy(), cv2.RETR_CCOMP, cv2.CHAIN_APPROX_NONE)
    # opencv hierarchy structure: [Next, Previous, First_Child, Parent]
    cv2.drawContours(image, contours,  -1, (0,255,0), 1)
    cv2.imshow("Contours", image)
    cv2.waitKey(0)
    
    hierarchy = hierarchy[0]    # Initializing the list
    circleContours = []
    i = 0
    nCirlces = 0    
    # Looping over contours one by one till the end, and for each contour, check if it satisfy our rules to be considered as a valid circle, if it's valid, add this contour to the list "circleContours for storage"
    for contour in contours:       
        (x, y, w, h) = cv2.boundingRect(contour)
        ar = w / float(h)
        if hierarchy[i][3] == -1 and w >= minCricleW and h >= minCricleH and 0.9 <= ar and ar <= 1.2: 
            epsilon = 0.01*cv2.arcLength(contour, True)
            approx = cv2.approxPolyDP(contour, epsilon, True)
            area = cv2.contourArea(contour)
            if ( (len(approx) > 8) & (len(approx) < 20) & (area >= minCricleArea) ):
                circleContours.append(contour)
                nCirlces = nCirlces + 1
        i = i + 1
    return [circleContours, nCirlces]   # Return a list containing another list, and the number of circles detected.

def isCircleChecked(mcOption, image):
    ulx = mcOption.centerX - mcOption.radius
    uly = mcOption.centerY - mcOption.radius
    lrx = mcOption.centerX + mcOption.radius
    lry = mcOption.centerY + mcOption.radius
    nPixels = (lrx-ulx)*(lry-uly)
    gray_image = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    circleImg = gray_image[int(uly):int(lry),int(ulx):int(lrx)]
    m = cv2.mean(circleImg)
    intensity = m[0]
    if m[0] < 160:
        mcOption.isChecked = True

image = cv2.imread("imgs/12.png")
circleContours, nCirlces = findCircleContours(image)

# Initialize a list of objects
mcOptions_ObjList = []
for i in range(nCirlces):
    aMcOption = mcOption(nCirlces-i, None, None)
    aMcOption.initCenters(circleContours[i])
    mcOptions_ObjList.append(aMcOption)
    
# Further filter out non-mcOption cirlces by calculating the mode of radius of all previously detected circles
mcOptions_ObjList = sorted(mcOptions_ObjList , key=lambda k: [k.centerY, k.centerX]) 
for i in range(nCirlces):
    radii.append(mcOptions_ObjList[i].radius)
radiiMode = mode(radii)
removed = 0
for mcOption in mcOptions_ObjList[:]:
    if not (mcOption.radius - radiusDelta < radiiMode and mcOption.radius + radiusDelta > radiiMode):
        mcOptions_ObjList.remove(mcOption)
        removed = removed + 1
nCirlces = nCirlces - removed

for i in range(nCirlces):
    isCircleChecked(mcOptions_ObjList[i], image)
    
# Show results
del circleContours[:]
for mcOption in mcOptions_ObjList[:]:
    circleContours.append(mcOption.circleContour)
    
cv2.drawContours(image, circleContours,  -1, (0,0,255), 1)
print(nCirlces, "Circles Detected")
cv2.imshow('Circles Detected',image)
cv2.waitKey(0)
cv2.destroyAllWindows()

x  = [mcOption.centerX for mcOption in mcOptions_ObjList]
y  = [mcOption.centerY for mcOption in mcOptions_ObjList]
cx = [mcOption.centerX for mcOption in mcOptions_ObjList if mcOption.isChecked == True]
cy = [mcOption.centerY for mcOption in mcOptions_ObjList if mcOption.isChecked == True]
plt.scatter(x, y, label='MC options dected')
plt.scatter(cx, cy, c='r', label='Checked MC options')
plt.gca().invert_yaxis()
plt.show()

# The followings use the K-Means Clustering algorithm, this works perfectly when all questions contain the same amount of options, 
# each options are evenly distrubuted, and each questions are evenly distributed and the distance between each questions should be 
# larger then that of betweening mcOptions

# K-means clustering of questions
def distance(p0, p1):
    return math.sqrt((p0[0] - p1[0])**2 + (p0[1] - p1[1])**2)
def average(x, y):
    if sum(x) == 0:
        x = [0, 0.1]
    if sum(y) == 0:
        y = [0, 0.1]
    return [sum(x)/len(x), sum(y)/len(y)]
def createList(k):
    mylist = []
    for i in range(k): 
        mylist.append(i)
    return mylist
K = 4    # K should be the number of questions.
C = createList(K)   # Create a list of centroids, centroids is the centroid of each cluster, each cluster is a group of mcOptions, a question in another word
height, width, _ = image.shape
# Randomly pick K number of centroids
for i in range(0, K):
    C[i] = ([random.randint(0,width), random.randint(0,height)])

# This big loop is the algorithmic representation of K-Means Clustering
lastC = None
while (True):
    # Assign each mcOption to the nearest centroid
    nPoints = len(mcOptions_ObjList)
    print("Number of MC options: ", nPoints)
    for i in range(0, nPoints):
        nearestCentroidDistance = 69696969
        for j in range(0, K):
            dist = distance( [mcOptions_ObjList[i].centerX, mcOptions_ObjList[i].centerY], C[j])
            if ( dist < nearestCentroidDistance):
                nearestCentroidDistance = dist
                mcOptions_ObjList[i].centroidID = j               
    for mcOption in mcOptions_ObjList[:]:
        print("[MC option] - ID: ", mcOption.ID, "\t- cluster ID: ", mcOption.centroidID)

    # Calculate new centroid for each cluster by taking the mean of the distances between each point and their assigned centroid within that cluster
    # untill no possible new centroid can be calculated
    for j in range(0, K):
        C[j] = average([mcOption.centerX for mcOption in mcOptions_ObjList if mcOption.centroidID == j], [mcOption.centerY for mcOption in mcOptions_ObjList if mcOption.centroidID == j])
    if (C == lastC):
        break
    lastC = C.copy()
