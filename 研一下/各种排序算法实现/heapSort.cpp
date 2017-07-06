// Source : https://github.com/longxi7997/LeetCode
// Author : Xi Long
// Email  : longxi7997@gmail.com
// Date   : 2017-07-04

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

void print(int a[] , int n /*, int i*/) {

	//cout << i << ":";
	
	for (int j = 0; j < n; j++)
	{
		cout << a[j] << " ";
	}
	cout << endl;
}

//下沉，生成最大堆
void heapDown(int mheap[] ,int curIndex , int last_index )
{
	int child = 2 * curIndex + 1;

	while (child <= last_index )
	{
		if ( (child + 1 )<= last_index &&  mheap[child] < mheap[child + 1])
			child++;
		if (mheap[child] > mheap[curIndex])
		{
			swap(mheap[child], mheap[curIndex]);
			curIndex = child;
			child = 2 * curIndex + 1;
		}
		else
			break;
	}

	//print(mheap, last_index + 1);

}

//将数组创建成 二叉堆
void createHeap(int a[], int nlength )
{
	
	int last_index = nlength-1;
	int parent_index = (last_index - 1) >> 1;

	//最大堆
	while (  parent_index >= 0 )
	{
		heapDown(a, parent_index , last_index);
		parent_index--;
	}

}

//堆排序
void heapSort(int mheap[], int nlength) 
{
	int last_index = nlength - 1;
	for (int i = 0 ; i < nlength ; i++)
	{
		swap( mheap[0] , mheap[last_index]);
		last_index--;
		heapDown(mheap , 0 , last_index);
	}
}

int main()
{

	

	//int a[10] = { 3,1,5,7,2,4,9,6,10,8 };
	//生成随机数测试
	int a[20000];
	for (int i = 0 ; i < 20000 ; i++)
	{
		a[i] = rand();
	}


	// 计时器
	clock_t clockBegin, clockEnd;
	clockBegin = clock();

	createHeap( a,  sizeof(a)/sizeof(a[0]) );

	heapSort( a,  sizeof(a)/sizeof(a[0]) );

	clockEnd = clock();

	print(a, 20000);

	cout << "耗时: " << (clockEnd - clockBegin) <<"ms";

	system("pause");
	return 0;

}


