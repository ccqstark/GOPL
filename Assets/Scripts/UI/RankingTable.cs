using System;
using TMPro;
using UnityEngine;

public class RankingTable : MonoBehaviour
{
    public GameObject TableContent; // 表格内容对象
    public GameObject RowPrefab; // 行预设

    private void Start()
    {
        // 从数据库中读取数据

        // 将数据添加到表格中
        for (int i = 0; i < 10; i++)
        {
            GameObject newRow = Instantiate(RowPrefab, TableContent.transform.position, TableContent.transform.rotation);
            newRow.name = "Row" + (i + 1);
            // 设置父组件
            newRow.transform.SetParent(TableContent.transform);
            newRow.transform.localScale = Vector3.one; // 设置缩放比例 1,1,1
            //设置预设实例中的各个子物体的文本内容
            newRow.transform.Find("Rank").GetComponent<TMP_Text>().text = "#" + (i + 1);
            newRow.transform.Find("Username").GetComponent<TMP_Text>().text = "Ccq" ;
            newRow.transform.Find("Score").GetComponent<TMP_Text>().text = "6666";
            newRow.transform.Find("TimeUsed").GetComponent<TMP_Text>().text = "00 : 27";
            newRow.SetActive(true);
        }
    }
}