# Multiple-Choice-OMR

This is an OMR program that takes a multiple choice sheet template as input, and automatically recognizes the MC options, and finally group them into questions and assign them into meaningful orders.

## Dependencies
This program is developed in Python3, and requires the following libraries:
1.  OpenCV
2.  Numpy
3.  MatPlotLib
4.  Tkinker

## Usage
To run the program:  
**Arch** >> python main.py  
**Debian** >> python3 main.py  
**Windows** >> py main.py  

## General flow
- Ask for the filename of the PDF template.
- Convert each PDF page to a PNG file.
- Process each PNG file by:
  - Denoise the image.
  - Find all contours in the image.
  - Filter and keep only those contours that qualified to be MC options.
  - Transform contours into meaningful X-Y coordinates.
  - Draw and show the user the identified MC options.
  - Use the K-Means Clustering algorithm to group MC options into questions
  - Print to show which group of question each option is assigned to.
  
## Features to be implemented
- More efficient and reliable method of assigning MC options into groups of questions.
- Ability to self-correct after user validation.
- More user-friendly interface, possibly more GUIs.
- Mark answered samples and do statistical outputs, for example, spreadsheet, chart and diagram.
- Implement Machine-Learning.
