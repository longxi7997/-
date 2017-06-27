// Source : https://github.com/longxi7997/LeetCode
// Author : Xi Long
// Email  : longxi7997@gmail.com
// Date   : 2017-06-27

/**********************************************************************************
* ��������
* 
* ����˼�룺
* �Ƚ�����������ļ�¼���зָ��Ϊ���������зֱ����ֱ�Ӳ������򣬴����������еļ�¼����������ʱ��
* �ٶ�ȫ���¼��������ֱ�Ӳ�������
*
* ϣ������Shell`s Sort�� (�ֽУ���С��������)
*
* ʱ�临�Ӷȣ�������
*
*
**********************************************************************************/

#include <stdio.h>
#include <iostream>
#include <vector>
#include <algorithm>
#include <math.h>
using namespace std;



void print(int a[] , int n , int i) {

	cout << i << ":";
	
	for (int j = 0; j < n; j++)
	{
		cout << a[j] << " ";
	}
	cout << endl;
}

void InsertSort(int a[], int n)
{

	for (int i = 1; i < n ; i++)
	{
		if (a[i] < a[i - 1])
		{
			int j = i - 1;
			int x = a[i];
			a[i] = a[i - 1];
			while (x < a[j])
			{
				a[j + 1] = a[j];
				j--;
			}
			a[j + 1] = x;
		}

		print(a, n, i);
	}
	print(a, n, n);
}

void shellInsertSort(int a[], int n , int dk)
{
	for (int i = dk ; i < n ; i++)
	{
		if (a[i] < a[i - dk])
		{
			int j = i - dk;
			int x = a[i];
			a[i] = a[i - dk];

			while ( x <a[j])
			{
				a[j + dk] = a[j];
				j -= dk;
			}
			a[j + dk] = x;

		}
		print(a, n, i);
	}
	print(a, n, n);
}

void shellSort( int a[] , int n )
{
	int dk = n / 2;
	//int dk = 1;
	while ( dk>=1 )
	{
		shellInsertSort(a, n, dk);

		dk = dk / 2;
	}
}

int main()
{

	int a[8] = { 3,1,5,7,2,4,9,6 };
 	
	//InsertSort( a,  sizeof(a)/sizeof(a[0]) );
	shellSort( a,  sizeof(a)/sizeof(a[0]) );


	system("pause");
	return 0;

}