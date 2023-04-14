using System;
using Db;
using TMPro;
using UnityEngine;

public class RankingTable : MonoBehaviour
{
    public GameObject TableContent; // 表格内容对象
    public GameObject RowPrefab; // 行预设

    private void Start()
    {
        // 构造SQL语句
        string querySQL = $@"SELECT u.username, tr.score, tr.time_used 
FROM top_record AS tr 
INNER JOIN user AS u ON tr.user_id = u.id
WHERE tr.level_id = {{0}}
ORDER BY tr.score DESC
LIMIT 10";
        querySQL = String.Format(querySQL, "1");
        // 从数据库中读取数据
        var dataResult = DbUtil.QueryData(querySQL);
        int i = 1;
        // 将数据添加到表格中
        while (dataResult.Read())
        {
            GameObject newRow = Instantiate(RowPrefab, TableContent.transform.position, TableContent.transform.rotation);
            newRow.name = "Row" + (i);
            // 设置父组件
            newRow.transform.SetParent(TableContent.transform);
            newRow.transform.localScale = Vector3.one; // 设置缩放比例 1,1,1
            //设置预设实例中的各个子物体的文本内容
            newRow.transform.Find("Rank").GetComponent<TMP_Text>().text = "#" + i;
            newRow.transform.Find("Username").GetComponent<TMP_Text>().text = dataResult[0].ToString();
            newRow.transform.Find("Score").GetComponent<TMP_Text>().text = dataResult[1].ToString();
            newRow.transform.Find("TimeUsed").GetComponent<TMP_Text>().text = dataResult[2].ToString();
            // 前三名赋予特殊颜色
            switch (i)
            {
                case 1:
                    newRow.transform.Find("Rank").GetComponent<TMP_Text>().color = Color.yellow;
                    newRow.transform.Find("Rank").GetComponent<TMP_Text>().fontStyle = FontStyles.Bold;
                    break;
                case 2:
                    newRow.transform.Find("Rank").GetComponent<TMP_Text>().color = Color.grey;
                    newRow.transform.Find("Rank").GetComponent<TMP_Text>().fontStyle = FontStyles.Bold;
                    break;
                case 3:
                    newRow.transform.Find("Rank").GetComponent<TMP_Text>().color = Color.green;
                    newRow.transform.Find("Rank").GetComponent<TMP_Text>().fontStyle = FontStyles.Bold;
                    break;
            }
            newRow.SetActive(true);
            i++;
        }
        DbUtil.CloseDbConn();
    }
}