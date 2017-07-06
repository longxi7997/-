// Source : https://github.com/longxi7997/LeetCode
// Author : Xi Long
// Email  : longxi7997@gmail.com
// Date   : 2017-07-05

/**********************************************************************************
* 堆排序
* 
* 基本思想：
* 
*
* 时间复杂度：O(nlogn )
*
*
**********************************************************************************/



#include <stdio.h>
#include <iostream>
#include <vector>
#include <algorithm>
#include <math.h>

#include <time.h>
using namespace std;

#define ARRAY_SIZE 200000


void print(int a[] , int n /*, int i*/) {

	//cout << i << ":";
	
	for (int j = 0; j < n; j++)
	{
		cout << a[j] << " ";
	}
	cout << endl;
}


////bubbleSort
//void bubbleSort1( int a[] , int nlength) 
//{
//	for(int i=0 ; i<nlength ; i++)
//		for (int j = nlength-1; j >i ; j--)
//		{
//			if (a[j] < a[j - 1])
//			{
//				//swap(a[j], a[j - 1]);
//				int tmp = a[j] ; a[j] = a[j - 1] ;  a[j - 1] = tmp;
//			}
//
//		}
//}
//
//void bubbleSort2(int a[], int n) {
//	for (int i = 0 ; i< n - 1; ++i)
//	{
//		for (int j = 0; j < n - i - 1; ++j)
//		{
//			if (a[j] > a[j + 1])
//			{
//				//int tmp = a[j] ; a[j] = a[j + 1] ;  a[j + 1] = tmp;
//				swap(a[j], a[j + 1]);
//			}
//		}
//	}
//}

void quickSort( int a[] , int nlength)
{

	if (nlength < 2)
		return;

	int i = 1, j = nlength - 1;

	//while ( i<j)
	while (i!=j)
	{
		//while ( a[j]>=a[0] && j>0)
		while (a[j] >= a[0] && i<j)
			j--;
		while (a[i] < a[0] && i<j )
			i++;
		if (i < j)
		{
			//swap(a[i], a[j]);
			int tmp = a[i];a[i] = a[j];a[j] = tmp;
		}
	}

	if (a[j] < a[0])
	{
		int tmp = a[0]; a[0] = a[j]; a[j] = tmp;
	}
		//swap(a[0], a[j]);

	quickSort( &a[0] , j );
	quickSort(&a[j+1], nlength-j-1);


}



int main()
{

	

	//int a[10] = { 3,1,5,7,2,4,9,6,10,8 };
	//生成随机数测试
	int a[ARRAY_SIZE];
	for (int i = 0 ; i < ARRAY_SIZE ; i++)
	{
		a[i] = rand();
	}
	//print(a, ARRAY_SIZE);

	// 计时器
	clock_t clockBegin, clockEnd;
	clockBegin = clock();

	quickSort( a,  sizeof(a)/sizeof(a[0]) );


	clockEnd = clock();

	//print(a, ARRAY_SIZE);

	cout << "耗时: " << (clockEnd - clockBegin) <<"ms";

	system("pause");
	return 0;

}


