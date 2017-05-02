#-*- coding:utf-8 -*-
import csv
import numpy

#  Source : https://github.com/longxi7997/Trivialness
#  Author : Xi Long
#  Email  : longxi7997@gmail.com
#  Date   : 2017-04-27

# 距离计算
# 此处用最近距离
# 欧氏距离 , 类间计算最近距离
def Distance( array1 , array2 ):

    minDistance = 99999999

    for i in range( len(array1) ):
        for j in range( len(array2) ):
            
            # 计算两个向量的距离
            temp = 0
            for k in range( len( array1[i] ) ):
                temp += (array1[i][k]-array2[j][k])*(array1[i][k]-array2[j][k])
            if temp < minDistance:
                minDistance = temp

    return minDistance

"""
# Jaccard 距离 , 类间计算最近距离
def Distance( array1 , array2 ):

    minDistance = 99999999

    for i in range( len(array1) ):
        for j in range( len(array2) ):
            
            # 计算两个向量的距离
            unin_set = 7
            join_set = 0
            temp = 0
            for k in range( len( array1[i] ) ):
                if array1[i][k] == array2[j][k] == '':
                    unin_set -=1
                elif array1[i][k] == array2[j][k]:
                    join_set+=1

            if unin_set==0:
                temp=1
            else:
                temp = 1-join_set/unin_set
            if temp < minDistance:
                minDistance = temp

    return minDistance
"""


                



# 聚合层次
def AggreationHerarchicalCluster( source_data , K ):
    
    cluster_count = len(source_data)

    if cluster_count <= K:
        return


    # 记录聚类过程 , idx, idx2, dist, sample_count
    
    mDendrogram = [] 

    # 现有的类别 ， 初始化为每个数据点
    curretClusters = []
    for i in range( len(source_data) ):
        curretClusters.append(i)

    # 距离矩阵，加快速度
    global DistanceArray
    DistanceArray = []
    # 初始化距离矩阵
    for i in range( len(source_data) ):
        array1 = []
        array1.append( source_data[i] )
        temp_array = []
        for j in range( 0, i+1 ):
            if i==j:
                temp_array.append( 99999999 )
            else:
                array2 = []
                array2.append(source_data[j])

                # 可以自己设定距离计算方式，修改 函数 Distance()
                temp_array.append( Distance( array1 , array2) )

        DistanceArray.append( temp_array )


    for loop_count in range( cluster_count-K ):

        print( "层次聚类循环次数： " ,loop_count)

        minDistance =99999999
        idx = [-1,-1,-1,-1,-1,-1]


        # 找到距离最小的
        for i in range( len( curretClusters) ):
            i = len( curretClusters)-1-i
            temp_dist = min( DistanceArray[i] )

            if temp_dist<minDistance:
                minDistance = temp_dist
                temp_index = DistanceArray[i].index( temp_dist )
                # print(len(curretClusters) , temp_index , i)
                idx[0] = min(curretClusters[ temp_index ] , curretClusters[i] )
                idx[1] = max(curretClusters[ temp_index ] , curretClusters[i] )
                idx[2] = minDistance
                idx[3] = -1    # 利用ETE绘制树状图这个属性就没用了
                idx[4] = min(temp_index,i)
                idx[5] = max(temp_index,i)


        del curretClusters[ idx[4] ]
        del curretClusters[ idx[5]-1 ]
        curretClusters.append( loop_count + cluster_count )

        # 更新距离矩阵
        for i in range( idx[5]+1 , len(DistanceArray) ):
            del DistanceArray[i][ idx[5] ]
        for i in range( idx[4]+1 , len(DistanceArray) ):
            del DistanceArray[i][ idx[4] ]
        del DistanceArray[ idx[5]]        
        del DistanceArray[ idx[4] ]
        


        # 更新过程
        idx.pop(4)
        idx.pop(4)
        mDendrogram.append( idx )

        # 更新距离矩阵
        for i in range( len(curretClusters)-1 ,len(curretClusters) ):
            temp_dist=[]
            # 获取最后一个类的数据
            temp_array = []
            array1 = []
            temp_array.append(curretClusters[ i])
            while ( len(temp_array)>0):
                a = temp_array.pop(0)
                if a>=cluster_count:
                    temp_array.append(mDendrogram[a-cluster_count][0])
                    temp_array.append(mDendrogram[a-cluster_count][1])
                else:
                    array1.append( source_data[a] )

            # print( "目前聚类数：" , curretClusters)
            for j in range( len(curretClusters) ):
                if i==j:
                    temp_dist.append( 99999999)
                else:
                    temp_array = []
                    array2 = []
                    temp_array.append(curretClusters[ j ])
                    while ( len(temp_array)>0):
                        a = temp_array.pop(0)
                        if a>=cluster_count:
                            temp_array.append(mDendrogram[a-cluster_count][0])
                            temp_array.append(mDendrogram[a-cluster_count][1])
                        else:
                            array2.append( source_data[a] )
                    temp_dist.append( Distance( array1 ,array2 ) )

            DistanceArray.append( temp_dist )

            # print( "距离矩阵增加: " , DistanceArray)


    return mDendrogram

#构建树结构
# 0:all
# 1:left
# 2:right
# 为了使得结果满足 python ETE模块的要求，可以进行树的可视化
# ETE 在线可视化网址 : http://etetoolkit.org/treeview/
def trans2Tree( array1 , Left_Right ):
    global mDendrogram

    global source_data
    mlen = len(source_data)
    S=""
    sLeft = ""
    sRight = ""

    if ( array1[0]>= mlen or array1[1]>= mlen):
        
        if array1[0]>=mlen:
            sLeft = "(" + trans2Tree(  mDendrogram[array1[0] - mlen] , 1 )
        else:
            sLeft = "(" + str(array1[0]+1) + ","


        if array1[1]>=mlen:
            sRight = trans2Tree(  mDendrogram[array1[1] - mlen] ,2)  + ")"
        else:
            sRight = str( array1[1]+1) + ")" + str( array1[2] )


    else:
        sLeft = "(" + str(array1[0]+1)  + ","        
        sRight = str( array1[1]+1) + ")" + str( array1[2])

    if Left_Right==1:
        S = sLeft + sRight + ","
    elif Left_Right == 2:
        S = sLeft + sRight + str(array1[2] )
    else:
        S = sLeft + sRight + str(array1[2] )

    return S

def parse_mDendrogram( mDendrogram  , mCluster ):
    
    # mCluster  设置聚类数量 ， 取出相应的聚类簇
    # mDendrogram 聚类的过程矩阵

    elements_count = len(mDendrogram)+1

    # 初始化聚类簇，一类
    curretClusters = []

    cluster_index_array = []
    cluster_index_array.append( 2*len(mDendrogram) )
    for i in range( mCluster-1):


        # 分裂最后合并的簇
        max_index = max(cluster_index_array) 


        cluster_index_array.remove(  max(cluster_index_array) )

        if max_index>=elements_count:
            
            cluster_index_array.append( mDendrogram[max_index-elements_count][0] )
            cluster_index_array.append( mDendrogram[max_index-elements_count][1] )



    # 根据类簇索引找到相应的元素
    for i in range(  len(cluster_index_array)  ):

        temp_array = []
        array1 = []
        temp_array.append(  cluster_index_array[i] )
        while ( len(temp_array)>0):
            a = temp_array.pop(0)
            if a>=elements_count:
                temp_array.append(mDendrogram[a-elements_count][0])
                temp_array.append(mDendrogram[a-elements_count][1])
            else:
                array1.append( a )
        curretClusters.append( array1 )

    return curretClusters


if __name__ == '__main__':


    # 所有的过程都以数组下角标区分每条记录，不需要给定记录ID
    # 聚类过程经过一定优化，运算速度会有一定提升

    """
    # 获得聚类结果 ， 根据写入文件的信息反向推到聚类结果
    rows = []

    with open( "mDendrogram_max.csv" ) as f:
        f_csv = csv.reader(f)

        for row in f_csv:
            temp = []
            temp.append( int(row[0]) )
            temp.append( int(row[1]) )

            rows.append( temp )

    curretClusters = parse_mDendrogram( rows , 5)
    print(curretClusters)
    """


    # K ：最终合并成几个聚类聚类的，一般情况都取 1
    K = 1
    global source_data
    source_data = []

    # source_data 作为输入的每个向量，格式如下：
    # source_data = [ [1],[2],[4],[5],[6] ]  一维
    # source_data = [ [1,1],[2,2],[4,4],[5,5],[6,6] ]  多维


    # 原始数据
    with open('test.csv') as f:
        f_csv = csv.reader(f)
        # headers = next(f_csv)
        for row in f_csv:
            source_data.append( row )


    global mDendrogram
    mDendrogram = AggreationHerarchicalCluster( source_data , K ) 


    # http://etetoolkit.org/treeview/
    global newick
    newick = ""

    newick = trans2Tree( mDendrogram[len(mDendrogram)-1] , 0 )


    # 树结构文件，可以利用 ETE 在线可视化直接展示树结构
    # http://etetoolkit.org/treeview/
    # 结构     ( cluster1,cluster2 )distance  
    # 例如: [ [1],[3],[4] ] 欧氏进行聚类， 得到 ( 1,)
    with open('tree.txt' , "w" , newline='') as f:
        f.writelines( newick )
    f.close()

    with open( 'mDendrogram.csv' , 'w' , newline='') as f:
        f_csv = csv.writer(f)

        f_csv.writerows( mDendrogram )
    f.close()


    

