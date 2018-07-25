########################################################################################################################
# The followings use the K-Means Clustering algorithm, this works perfectly when all questions contain the same 
# amount of options, each options are evenly distrubuted, and each questions are evenly distributed and the distance 
# between each questions should be larger then that of betweening mcOptions.
########################################################################################################################
# NOTE: Such implementation is still experimental and is very unreliable at this stage.
########################################################################################################################
import math
import random

def distance(p0, p1):
    return math.sqrt((p0[0] - p1[0])**2 + (p0[1] - p1[1])**2)
    
def average(x, y):
    if sum(x) == 0:
        retX = 0
    else:
        retX = sum(x)/len(x)    
    if sum(y) == 0:
        retY = 0
    else:
        retY = sum(y)/len(y)
    return [retX, retY]
       
def kMeanClustering(image, objList):
    input("cluster begin")    
    K = 4
    C = []
    height, width, _ = image.shape
    for i in range(0, K):
        C.append([random.randint(0,width), random.randint(0,height)])
    # Cluster optimization
    nPoints = len(objList)
    print("Number of MC options: ", nPoints)
    lastC = None
    while (True):
        # Assign each mcOption to the nearest centroid.
        for i in range(0, nPoints):
            nearestCentroidDistance = 69696969
            for j in range(0, K):
                print("J: ", j)
                print("C[j]: ", C[j])
                dist = distance( [objList[i].centerX, objList[i].centerY], C[j] )
                if (dist < nearestCentroidDistance):
                    nearestCentroidDistance = dist
                    objList[i].centroidID = j               
        for mcOption in objList[:]:
            print("[MC option] - ID: ", mcOption.ID, "\t- cluster ID: ", mcOption.centroidID)

        # Calculate new centroid for each cluster by taking the mean of the distances between each point and their 
        # assigned centroid within that cluster untill no possible new centroid can be calculated.
        for j in range(0, K):
            C[j] = average([mcOption.centerX for mcOption in objList if mcOption.centroidID == j], [mcOption.centerY for mcOption in objList if mcOption.centroidID == j])
        if (C == lastC):
            break
        lastC = C.copy()