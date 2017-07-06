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





void merge(int a[], int mSorted[], int mfirst, int mmid, int mlast)
{

	int tmpfirst = mfirst , tmpmid = mmid+1 ;
	int k = 0;
	
	while (tmpfirst<= mmid && tmpmid<=mlast  )
	{
		mSorted[k++] = a[tmpfirst] < a[tmpmid] ? a[tmpfirst++] : a[tmpmid++];
	}
	while (tmpfirst <= mmid )
	{
		mSorted[k++] = a[tmpfirst++];
	}
	while (tmpmid <=mlast )
	{
		mSorted[k++] = a[tmpmid++];
	}
	

	for (int i = 0 ; i < k ; i++)
	{
		a[i] = mSorted[i];
	}

	//print(a, ARRAY_SIZE);

}

void mergeSort( int a[] , int nlength ,int mSorted[])
{
	int first = 0, last = nlength - 1, mid = last/ 2;

	if ( nlength > 1 )
	{
		mergeSort(  &a[0] ,  mid+1 , &mSorted[0]);
		mergeSort(  &a[mid+1], (last-mid), &mSorted[mid+1]);

		merge(a, mSorted, first, mid, last);

	}

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

	int *sorted = new int[ARRAY_SIZE];
	
	mergeSort( a,  sizeof(a)/sizeof(a[0]) , sorted );


	clockEnd = clock();

	//print( a, ARRAY_SIZE);

	delete[]sorted;

	cout << "耗时: " << (clockEnd - clockBegin) <<"ms";

	system("pause");

	return 0;

}


