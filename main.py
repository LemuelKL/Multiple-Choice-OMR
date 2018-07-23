import cv2
import numpy as np
from collections import Counter
import itertools
from statistics import mode
from pprint import pprint
import sys

circle_radius = 0
radius = []
centers = []
mcOptions = []  # for storing a list of class mcOption objects
nMCOptions = -1

class mcOption:
    def __init__(self, ID, question_id, option_id, center_x_coord, center_y_coord, radius):
        self.ID = ID
        self.question_id = question_id
        self.option_id = option_id
        self.center_x_coord = center_x_coord
        self.center_y_coord = center_y_coord
        self.radius = radius

    question_value = -1
    option_value = -1

def process_image(image):
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    blurry = cv2.GaussianBlur(gray, (3, 3), 1)
    adapt_thresh = cv2.adaptiveThreshold(blurry, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY_INV, 11, 2)
    return adapt_thresh

def count_options(image):
    cv2.imshow("Orginal",image)
    cv2.waitKey(0)

    processed_image = process_image(image)
    cv2.imshow("processed_image",processed_image)
    cv2.waitKey(0)
    _, contours, hierarchy = cv2.findContours(processed_image.copy(), cv2.RETR_CCOMP, cv2.CHAIN_APPROX_SIMPLE)
    #opencv hierarchy structure: [Next, Previous, First_Child, Parent]
    hierarchy = hierarchy[0]
    print("All Hierarchies: ", *hierarchy, sep = "\n")
    img = image
    cv2.drawContours(img, contours, -1, (0,255,0), 1)
    cv2.imshow('All Contours', img)
    cv2.waitKey(0)

    optionCnts = []
    optionCntsRects = []
    
    i = 0
    for c in contours:
        (x, y, w, h) = cv2.boundingRect(c)
        ar = w / float(h)
        if hierarchy[i][3] == -1 and w >= 18 and h >= 18 and 0.9 <= ar and ar <= 1.2:
            radius.append(w/2)
            optionCnts.append(c)
            optionCntsRects.append(cv2.boundingRect(c))     
        i = i+1

    cv2.drawContours(image, optionCnts, -1, (0, 0, 255), 1)
    cv2.imshow("Circles Detected",image.copy())
    cv2.waitKey(0)
    cv2.destroyAllWindows()

    optionCntsRects.reverse()
    print("optionCntsRects: ")
    print(*optionCntsRects, sep = "\n")
    ret = [optionCnts, optionCntsRects]
    return ret

def rect_to_center(rect):
    centers = []
    for r in rect:
        centers.append([r[0]+r[2]/2, r[1]+r[3]/2])
    return centers

def print_mcOption_detail(ID):
    pprint(vars(mcOptions[ID]))

def sort_by_xy(mcOptions):
    return sorted(mcOptions , key=lambda k: [k.center_y_coord, k.center_x_coord])

def assign_mc_to_question(mcOptions):
    first_option = mcOptions[0]
    mcOptions = sort_by_xy(mcOptions)
    global nMCOptions
    for i in range(nMCOptions):
        print_mcOption_detail(i)

    tmp_question_id = 0
    for i in range(nMCOptions):
        print_mcOption_detail(i)

def main():
    iamge_name = input("File name of the paper: ")
    #iamge_name = sys.argv[1]
    image = cv2.imread(iamge_name) 
    ret = count_options(image)
    global nMCOptions
    nMCOptions = len(ret[0])
    print("There are ", nMCOptions, " multiple choice options")
    if len(radius) > 0:
        print("All radii of detected circles: " ,radius)
        print("Occurrences of radii: " ,Counter(radius))
        print("Mode of radii: " ,mode(radius))
        global circle_radius
        circle_radius = mode(radius)

    global centers
    centers = rect_to_center(ret[1])
    print(*centers, sep = "\n")

    global mcOptions
    for i in range(nMCOptions):
        mcOptions.append(mcOption(i, -1, -1, centers[i][0], centers[i][1], circle_radius))
        print_mcOption_detail(i)
        
    assign_mc_to_question(mcOptions)
    
main()
