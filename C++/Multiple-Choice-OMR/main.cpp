#include <opencv2/core/core.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <iostream>
#include <string>
#include <windows.h>
#include <unistd.h>
#include <vector>
#include <sys/types.h>
#include <sys/stat.h>


int nCircles=0;
const int minCircleW = 16;
const int minCircleH = 16;
const int minCircleArea = ((minCircleW+minCircleH)/4)*((minCircleW+minCircleH)/4)*3.1;

using namespace cv;
using namespace std;

Mat processImg(cv::Mat img){
    Mat blurredImg;
    Mat adaptThreshImg;
    cv::GaussianBlur(img, blurredImg, Size(3,3), 1);
    cv::adaptiveThreshold(blurredImg, adaptThreshImg, 255, cv::ADAPTIVE_THRESH_GAUSSIAN_C, cv::THRESH_BINARY_INV, 11, 2);
    return adaptThreshImg;
}

float calModeRadius(vector<float> v)
{
    sort(v.begin(), v.end());
    float prev = v.back();
    float mode;
    int maxcount = 0;
    int currcount = 0;
    unsigned int i;
    for (i=0;i<v.size();i++) {
        float n = v[i];
        if (n == prev) {
            ++currcount;
            if (currcount > maxcount) {
                maxcount = currcount;
                mode = n;
            }
        } else {
            currcount = 1;
        }
        prev = n;
    }
    return mode;
}

 vector<vector<Point> > findCircleContours(Mat img){
    vector<float> radii;
    float modeRadius;
    vector<vector<Point> > circleContours;
    Mat proccessedImg = processImg(img);
    vector<vector<Point> > contours;
    vector<Vec4i> hierarchy;
    cv::findContours(proccessedImg, contours, hierarchy, RETR_CCOMP, CHAIN_APPROX_NONE);

    vector<Rect> boundRect(contours.size());
    unsigned int c;
    for ( c=0 ; c < contours.size() ; c++ ){
        boundRect[c] = cv::boundingRect(contours[c]);
        float ar = boundRect[c].width / float(boundRect[c].height);
        if ( hierarchy[c][3] == -1 && boundRect[c].width >= minCircleW && boundRect[c].height >= minCircleH && ar >= 0.9 && ar <= 1.2 ){
            double epsilon = 0.01 * cv::arcLength(contours[c], TRUE);
            vector<Point> approxCurve;
            cv::approxPolyDP(contours[c], approxCurve, epsilon, TRUE);
            double area = cv::contourArea(contours[c]);
            if ( approxCurve.size() > 8 && approxCurve.size() < 20 && area > minCircleArea ){
                Point2f center;
                float radius;
                minEnclosingCircle(contours[c], center, radius);
                if ( arcLength(contours[c], TRUE) > 2.9*2*radius && arcLength(contours[c], TRUE) < 3.3*2*radius ){
                    circleContours.push_back(contours[c]);
                    radii.push_back(radius);
                    nCircles++;
                }
            }
        }
    }

    modeRadius = calModeRadius(radii);
    int i=0;
    for(auto const& radius: radii){
        if (abs(radius-modeRadius) > 1){
            circleContours.erase(circleContours.begin()+i);
            i--;
        }
        i++;
    }

    cout << "Mode of radii: " << modeRadius << endl;
    cout << "Number of circles: " << nCircles << endl;
    cvtColor(img,img,COLOR_GRAY2BGR);
    cv::drawContours(img, circleContours, -1, Scalar(0,255,0), 2);
    Mat imgS;
    Size s = img.size();
    cv::resize(img, imgS, cv::Size(s.width/3, s.height/3));
    cv::imshow("Contours", imgS);
    cv::waitKey(0);
    cv::destroyAllWindows();

    return circleContours;
}

int singleImgLogic(cv::Mat img){
    findCircleContours(img);
    return 0;
}

void get_all_files_names_within_folder(string folder, vector<string> &names, string fExt)
{
    string search_path = folder + "/*." + fExt;
    WIN32_FIND_DATA fd;
    HANDLE hFind = ::FindFirstFile(search_path.c_str(), &fd);
    if(hFind != INVALID_HANDLE_VALUE) {
        do {
            if(! (fd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) ) {
                names.push_back(folder+"\\"+fd.cFileName);
            }
        }while(::FindNextFile(hFind, &fd));
        ::FindClose(hFind);
    }
}
bool dirExists(const std::string& dirName_in)
{
    DWORD ftyp = GetFileAttributesA(dirName_in.c_str());
    if (ftyp == INVALID_FILE_ATTRIBUTES)
        return false;
    if (ftyp & FILE_ATTRIBUTE_DIRECTORY)
        return true;
    return false;
}

vector<string> getFullPathNameOfImgToProcess()
{
    string imgPrefix(17, ' ');
    cout << endl << "Enter name of Image Folder: " << endl;
    cin >> imgPrefix;
    string workingDir = "D:\\Users\\Lemuel\\Documents\\GitHub\\Multiple-Choice-OMR\\C++\\Multiple-Choice-OMR\\";

    string imgFolder = workingDir + imgPrefix;
    if(!dirExists(imgFolder)){
        cout << "Cannot access " << imgFolder << endl;
        cout << "Folder possibly does not exist!" << endl ;
    }

    vector<string> fNames;
    get_all_files_names_within_folder(workingDir + imgPrefix, fNames, "png");

    if(fNames.size()<=0)
        return vector<string>();

    return fNames;
}

int main()
{
    unsigned int i;
    vector<string> pathToImgs;

    do{
        pathToImgs = getFullPathNameOfImgToProcess();
        if (pathToImgs.size()==0)
            cout << "Folder is empty!" << endl;
    } while (pathToImgs.size()==0);

    cout << "Images to be processed" << endl;
    for (i=0; i < pathToImgs.size(); i++){
        cout << pathToImgs[i] << endl;
        singleImgLogic(cv::imread(pathToImgs[i], 0));
    }

    return 0;
}

