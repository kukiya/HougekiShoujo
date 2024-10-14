using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class MVPIllust : MonoBehaviour
{
    Vector3 startPos = Vector3.left * 1920;
    Vector3 endPos = Vector3.left * 160;

    Vector3 dropShadowPos = new Vector3(16, -16, 0);

    Image[] charaIllusts = new Image[2];

    float timer;
    const float arriveTime = 0.75f;
    bool arrive;
    public bool GetArrive() { return arrive; }

    float turnBackTimer;
    const float turnBackTime = 0.5f;
    bool turnBack;
    bool turnBackArrive;
    public void SetTurnBack() { turnBack = true; }
    public bool GetTurnBackComplete() { return turnBackArrive; }

    ResultScoreBoard.KDFData kdf;
    [SerializeField] GameObject nameBorderPrefab;

    public void SetData(ResultScoreBoard.KDFData _kdf, PlayerData _pd)
    {
        kdf = _kdf;

        //初期座標
        transform.localPosition = startPos;
        //イラストを適用するImageコンポーネントの参照
        for (int i = 0; i < 2; i++) { charaIllusts[i] = transform.GetChild(i).GetComponent<Image>(); }

        //キャライラストの適用
        Sprite illust = _pd.GetCharacterAnimData().GetCharaIllust();
        for (int i = 0; i < 2; i++) { charaIllusts[i].sprite = illust; }

    }
    void Update()
    {
        if (turnBack && !turnBackArrive)
        {
            turnBackTimer += Time.deltaTime;
            if (turnBackTimer >= turnBackTime)
            {
                turnBackTimer = turnBackTime;
                turnBackArrive = true;
            }
            float nowRate = Mathf.Pow(turnBackTimer / turnBackTime, 2);
            transform.localPosition = Vector3.Lerp(endPos, startPos, nowRate);
        }
        else if (!arrive)
        {
            timer += Time.deltaTime;
            if (timer >= arriveTime)
            {
                timer = arriveTime;
                arrive = true;
                GameObject obj = Instantiate(nameBorderPrefab, transform);
                obj.GetComponent<NameBorder>().SetData(kdf);
            }
            float nowRate = Mathf.Sqrt(timer / arriveTime);
            transform.localPosition = Vector3.Lerp(startPos, endPos, nowRate);
            charaIllusts[0].transform.localPosition = dropShadowPos * nowRate;
        }
    }
}
