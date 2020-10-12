using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentLogList : MonoBehaviour
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private Log _log;
    [SerializeField]
    private TeacherPhotonReceiver _teacherPhotonReceiver;

    private List<Log> _logList;

    private void Start()
    {
        _logList = new List<Log>();
    }
    private void OnEnable()
    {
        //Subscribe to set log event
        _teacherPhotonReceiver.SetLog += AddNewLog;
    }
    private void OnDisable()
    {
        _teacherPhotonReceiver.SetLog -= AddNewLog;
    }
    private void AddNewLog(string studentName,string logInfo)
    {
        string _logText = studentName + " " + logInfo;
        Log newLog = Instantiate(_log, _content);
        if (newLog != null)
        {
            //move the new log to the top of the list
            newLog.transform.SetAsFirstSibling();
            newLog.SetLogText(_logText);
            _logList.Add(newLog);
        }
    }
}
