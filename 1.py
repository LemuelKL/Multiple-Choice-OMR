import cv2
import numpy as np

img = cv2.imread('1.png',0)
#img = cv2.medianBlur(img,5)
cimg = cv2.cvtColor(img,cv2.COLOR_GRAY2BGR)

circles = cv2.HoughCircles(img,cv2.HOUGH_GRADIENT,1,1,
                            param1=50,param2=26,minRadius=0,maxRadius=50)

circles = np.uint16(np.around(circles))
j=0
for i in circles[0,:]:
    if i[2]:
        # draw the outer circle
        cv2.circle(cimg,(i[0],i[1]),i[2],(255,255,0),2)
        # draw the center of the circle
        cv2.circle(cimg,(i[0],i[1]),2,(255,0,0),1)
        j=j+1

print(j)
cv2.imshow('detected circles',cimg)
cv2.waitKey(0)
cv2.destroyAllWindows()
