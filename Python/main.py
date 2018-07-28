import cv2
import numpy as np
from statistics import mode
import sys
import matplotlib.pyplot as plt
from PDF_to_PNG import convertPDF
import os
import time
import kMean

radii = []
# Define acceptable error
radiusDelta = 1

# Define acceptable range of dimension sizes, 15-20 usually.
minCricleW = 16
minCricleH = 16
minCricleArea = ((minCricleW+minCricleH)/4)*((minCricleW+minCricleH)/4)*3.1

class _mcOption:
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
        self.ID = ID
        self.questionID = questionID
        self.optionID = optionID
        
    def initCenters(self, circleContour):
        self.circleContour = circleContour
        self.centerX, self.centerY, self.radius = extractFromCricleContour(circleContour)
    
def extractFromCricleContour(circleContour):
    (x, y, w, h) = cv2.boundingRect(circleContour)  # This boudingRect function in opencv takes in a "contour" datatype
    return [x+w/2, y+h/2, w/2]                      # and return 4 values that form a bouding rectangle of the contour.

def processImage(image):
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    blurry = cv2.GaussianBlur(gray, (3, 3), 1)
    adapt_thresh = cv2.adaptiveThreshold(blurry, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY_INV, 11, 2)
    return adapt_thresh

def findCircleContours(image):
    processed_image = processImage(image)
    # cv2.fincContours return 3 values, 1st one not needed, "contours" contains all contours in the given image, 
    # "hierarchy", refer to first glance https://docs.opencv.org/3.4.0/d9/d8b/tutorial_py_contours_hierarchy.html
    # for understanding, basically it describe a "level" relationship between contours and contours.
    # Opencv hierarchy structure: [Next, Previous, First_Child, Parent]
    _, contours, hierarchy = cv2.findContours(processed_image.copy(), cv2.RETR_CCOMP, cv2.CHAIN_APPROX_NONE)
    cv2.drawContours(image, contours,  -1, (0,255,0), 1)
    cv2.imshow("Contours", image)
    cv2.waitKey(0)
    
    hierarchy = hierarchy[0]
    circleContours = []
    i = 0
    nCirlces = 0    
    # Looping over contours one by one till the end, and for each contour, check if it satisfy our rules to be 
    # considered as a valid circle, if it's valid, add this contour to the list "circleContours for storage".
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

def filterBadCricles(objList):
    initN = len(objList)
    objList = sorted(objList , key=lambda k: [k.centerY, k.centerX]) 
    for i in range(nCirlces):
        radii.append(objList[i].radius)
    radiiMode = mode(radii)
    removed = 0
    for mcOption in objList[:]:
        if not ( mcOption.radius - radiusDelta < radiiMode and mcOption.radius + radiusDelta > radiiMode ):
            objList.remove(mcOption)
            removed = removed + 1
    return [objList, initN - removed]
    
def makeMCs(nCirlces):
    mcOptions_ObjList = []
    for i in range(nCirlces):
        instance = None
        instance = _mcOption(nCirlces-i, None, None)
        instance.initCenters(circleContours[i])
        mcOptions_ObjList.append(instance)
    return mcOptions_ObjList

################################################################################
print("[1]Multi-page PDD")
print("[2]Single PNG")
_mode = input("[?] = ")
if _mode == "1":
    imgFolder = "./imgs"
    if not os.path.exists(imgFolder):
        os.makedirs(imgFolder)
    while( os.path.isfile(input("Please enter the filename of the PDF template: ")) == False ):
        print("FILE DOES NOT EXIST! Please Try Again!")
    imgPath = imgFolder + "/" + time.strftime("%Y%m%d-%H%M%S") + "-" + pdfName[:-4]
    os.makedirs(imgPath)
    nPage, pngNames = convertPDF(pdfName, imgPath)
    images = []
    for name in pngNames:
        print(imgPath + name)
        images.append(cv2.imread(imgPath + "/" + name))
if _mode == "2":
    images = []
    images.append(cv2.imread("imgs/Test1.png"))      
################################################################################

currentPage = 0    
for image in images:
    currentPage = currentPage + 1
    input("Press <ENTER> to begin processing the current page [" + str(currentPage) + "]: ")
    
    circleContours, nCirlces = findCircleContours(image)
    mcOptions_ObjList = makeMCs(nCirlces)        
    mcOptions_ObjList, nCirlces = filterBadCricles(mcOptions_ObjList)
    
    # Check to see if cricle is checked
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
    cx = [mcOption.centerX for mcOption in mcOptions_ObjList if mcOption.isChecked == True] #checked
    cy = [mcOption.centerY for mcOption in mcOptions_ObjList if mcOption.isChecked == True] #checked
    plt.scatter(x, y, label='MC options dected')
    plt.scatter(cx, cy, c='r', label='Checked MC options')
    plt.gca().invert_yaxis()
    plt.show()

    groupedList, K, height, width = kMean.kMeanClustering(image, mcOptions_ObjList, 5)
    for i in range(0, K):
        plotx = [mcOption.centerX for mcOption in groupedList if mcOption.centroidID == i]
        ploty = [mcOption.centerY for mcOption in groupedList if mcOption.centroidID == i]
        plt.scatter(plotx,ploty,label=i)
        plt.xlim(0, width)
        plt.ylim(0, height)
        plt.gca().invert_yaxis()
        plt.show()
