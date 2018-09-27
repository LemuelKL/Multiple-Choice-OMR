#include <cstdlib>
#include <math.h>
#include <vector>

int distance(int p1x, int p1y, int p2x, int p2y)
{
    return sqrt(abs(pow((p1x - p2x), 2)) + abs(pow((p1y - p2y), 2)));
}
/*
vector<choice> kMeanClustering()
{

    int i = 0;
}
*/
