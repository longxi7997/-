// Source : https://github.com/longxi7997/LeetCode
// Author : Xi Long
// Email  : longxi7997@gmail.com
// Date   : 2017-06-27

/**********************************************************************************
* ��������
* 
* ����˼�룺
* ��ð���������ƣ�������һ�ε�λ��ÿ���ҵ���Ԫ�ص�λ�ã���������һλ
* 
* ʱ�临�Ӷȣ�O��n ^ 2��.
*
* �����Ĳ��������ж��ֲ�������2 - ·��������
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

void InsertSort(int a[]  , int n)
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

		print(a , n , i);
	}
	print(a, n, n);
}


int main()
{

	int a[8] = { 3,1,5,7,2,4,9,6 };
 	
	InsertSort( a,  sizeof(a)/sizeof(a[0]) );


	system("pause");
	return 0;

}