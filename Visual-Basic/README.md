## Dependencies
This program is developed in under Visual Studio Community 2017 32bit on Windows 10.  
The following libraries / wrappers are used:
1.	GhostScript	9.23
2.	GhostScript.NET	1.2.1
3.	ZedGraph 5.1.7
4.	EMGU.CV	3.4.1.2976

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
