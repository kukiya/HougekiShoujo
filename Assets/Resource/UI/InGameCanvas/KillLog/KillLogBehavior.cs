using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class KillLogBehavior : MonoBehaviour
{
    Text killPlayer;
    Text killText;
    Text deadPlayer;

    Vector3 startPos = Vector3.right * 400;
    Vector3 endPos = Vector3.zero;

    int logNum = 0;

    float timer;
    const float posTime = 0.25f;
    const float colorChangeTime = 5;
    const float lifeTime = 6;

    readonly Color[] constColor = new Color[2] { new Color(0.1255f, 0.3137f, 0.8941f), new Color(1, 0.125f, 0.125f, 1) };
    Color killColor;
    Color deadColor;

    const float killVoiceTime = 0.5f;
    bool playKillVoice;
    AudioClip killVoice;

    public void SetText(Player _player)
    {
        killPlayer = transform.GetChild(0).GetComponent<Text>();
        killText = transform.GetChild(1).GetComponent<Text>();
        deadPlayer = transform.GetChild(2).GetComponent<Text>();

        int killer = _player.GetKiller();
        int deadMan = _player.GetPlayerID();

        MachingRoomData.RoomData killerRoomData = OSCManager.OSCinstance.GetRoomData(killer);
        MachingRoomData.RoomData deadManRoomData = OSCManager.OSCinstance.GetRoomData(deadMan);

        killPlayer.text = killerRoomData.playerName;
        deadPlayer.text = deadManRoomData.playerName;

        int killerTeam = killerRoomData.myTeamNum;
        int deadManTeam = deadManRoomData.myTeamNum;
        killColor = constColor[killerTeam];
        deadColor = constColor[deadManTeam];
        killPlayer.color = killColor;
        deadPlayer.color = deadColor;

        //自決なら
        if (killer == deadMan)
        {
            SoundManager.PlayVoice(_player.GetPlayerData().GetPlayerVoiceData().GetDamage());
        }
        //FFなら
        else if (killerTeam == deadManTeam)
        {
            SoundManager.PlayVoice(_player.GetPlayerData().GetPlayerVoiceData().GetDamageFF());
            killVoice = Managers.instance.gameManager.GetPlayer(killer).GetPlayerData().GetPlayerVoiceData().GetFriendlyFire();
        }
        else
        {
            SoundManager.PlayVoice(_player.GetPlayerData().GetPlayerVoiceData().GetDamage());
            killVoice = Managers.instance.gameManager.GetPlayer(killer).GetPlayerData().GetPlayerVoiceData().GetKill();
        }

    }
    public void LogNumAdd() { logNum++; }

    void Update()
    {
        timer += Time.deltaTime;

        float posValue = Mathf.Clamp01(timer / posTime);
        Vector3 addPos = Vector3.up * (logNum * -136);
        transform.localPosition = Vector3.Lerp(startPos + addPos, endPos + addPos, posValue);

        float colorValue = Mathf.Clamp01(1.0f - (timer - colorChangeTime));
        Color multiplyRate = new Color(1, 1, 1, colorValue);
        killPlayer.color = killColor * multiplyRate;
        killText.color = Color.white * multiplyRate;
        deadPlayer.color = deadColor * multiplyRate;

        if (timer > lifeTime) { Destroy(gameObject); }


        if (killVoice == null) { return; }
        if (playKillVoice) { return; }
        if (timer > killVoiceTime) { SoundManager.PlayVoice(killVoice); }
    }
}
