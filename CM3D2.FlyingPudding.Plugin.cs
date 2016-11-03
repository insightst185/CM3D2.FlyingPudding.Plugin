using System;
using System.IO;
using System.Xml;
using System.Text;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace CM3D2.FlyingPudding.Plugin
{
    [PluginFilter("CM3D2x64"),
    PluginFilter("CM3D2x86"),
    PluginFilter("CM3D2VRx64"),
    PluginName("FlyingPudding"),
    PluginVersion("0.0.0.1")]
    public class FlyingPudding : PluginBase
    {
        private XmlManager xmlManager;
        private int maidNo = 0;
        private Vector3 move = new Vector3();
        private Vector3 rotate = new Vector3();

        private enum TargetLevel
        {
            Scene_hoge = 27
        }

        private enum DebugTargetLevel
        {
            Scene_Title = 9
        }

        private void Awake()
        {
        }

        private void OnLevelWasLoaded(int level)
        {
            if (Enum.IsDefined(typeof(DebugTargetLevel), Application.loadedLevel) ||
                Enum.IsDefined(typeof(TargetLevel), Application.loadedLevel))
            {
                 xmlManager = new XmlManager();

                 maidNo = 0;
            }
        }

        private void Update()
        {
            // Key Debug
            if (Enum.IsDefined(typeof(DebugTargetLevel), Application.loadedLevel))
            {
                if (Input.GetKeyDown(xmlManager.KbMoveForward)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ MoveForward ]");
                }
                if (Input.GetKeyDown(xmlManager.KbMoveBackward)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ MoveBackward ]");
                }
                if (Input.GetKeyDown(xmlManager.KbMoveLeft)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ MoveLeft ]");
                }
                if (Input.GetKeyDown(xmlManager.KbMoveRight)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ MoveRight ]");
                }
                if (Input.GetKeyDown(xmlManager.KbMoveUp)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ MoveUp ]");
                }
                if (Input.GetKeyDown(xmlManager.KbMoveDown)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ MoveDown ]");
                }
                if (Input.GetKeyDown(xmlManager.KbRotateUp)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ RotateUp ]");
                }
                if (Input.GetKeyDown(xmlManager.KbRotateDown)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ RotateDown ]");
                }
                if (Input.GetKeyDown(xmlManager.KbRotateLeft)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ RotateLeft ]");
                }
                if (Input.GetKeyDown(xmlManager.KbRotateRight)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ RotateRight ]");
                }
                if (Input.GetKeyDown(xmlManager.KbSlopeLeft)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ SlopeLeft ]");
                }
                if (Input.GetKeyDown(xmlManager.KbSlopeRight)){
                    Debug.Log("FlyingPudding.Plugin:Key Debug [ SlopeRight ]");
                }
            }

            // Maid Move
            if (Enum.IsDefined(typeof(TargetLevel), Application.loadedLevel))
            {
                Maid maid = GameMain.Instance.CharacterMgr.GetMaid(maidNo);
                if(maid != null){
                    // ジョイスティック入力
                    move.x = - Input.GetAxis(xmlManager.JsMoveX);
                    move.z = - Input.GetAxis(xmlManager.JsMoveY);
                    
                    // キーボード入力
                    move.y = (Input.GetKey(xmlManager.KbMoveUp))       ?  xmlManager.MoveSpeed :
                             (Input.GetKey(xmlManager.KbMoveDown))     ? -xmlManager.MoveSpeed : 0.0f;
                    if (move.z * move.z < 0.2f * 0.2f){
                      move.z = (Input.GetKey(xmlManager.KbMoveForward))  ?  xmlManager.MoveSpeed :
                               (Input.GetKey(xmlManager.KbMoveBackward)) ? -xmlManager.MoveSpeed : 0.0f;
                    }
                    else{
                      move.z = move.z * xmlManager.MoveSpeed;
                    }
                    if (move.x * move.x < 0.2f * 0.2f){
                      move.x = (Input.GetKey(xmlManager.KbMoveRight))    ? -xmlManager.MoveSpeed :
                               (Input.GetKey(xmlManager.KbMoveLeft))     ?  xmlManager.MoveSpeed : 0.0f;
                    }
                    else{
                      move.x = move.x * xmlManager.MoveSpeed;
                    }
                    rotate.y = (Input.GetKey(xmlManager.KbRotateRight)) ?  xmlManager.RotateSpeed :
                               (Input.GetKey(xmlManager.KbRotateLeft))  ? -xmlManager.RotateSpeed : 0.0f;
                    rotate.x = (Input.GetKey(xmlManager.KbRotateUp))    ?  xmlManager.RotateSpeed :
                               (Input.GetKey(xmlManager.KbRotateDown))  ? -xmlManager.RotateSpeed : 0.0f;
                    rotate.z = (Input.GetKey(xmlManager.KbSlopeRight))  ?  xmlManager.RotateSpeed :
                               (Input.GetKey(xmlManager.KbSlopeLeft))   ? -xmlManager.RotateSpeed : 0.0f;

                    maid.SetPos(maid.gameObject.transform.localPosition + move);
                    maid.SetRot(maid.GetRot() + rotate);
                }
            }
        }


        //------------------------------------------------------xml--------------------------------------------------------------------
        private class XmlManager
        {
            public KeyCode KbMoveForward = KeyCode.None;
            public KeyCode KbMoveBackward = KeyCode.None;
            public KeyCode KbMoveLeft = KeyCode.None;
            public KeyCode KbMoveRight = KeyCode.None;
            public KeyCode KbMoveUp = KeyCode.None;
            public KeyCode KbMoveDown = KeyCode.None;
            public KeyCode KbRotateUp = KeyCode.None;
            public KeyCode KbRotateDown = KeyCode.None;
            public KeyCode KbRotateLeft = KeyCode.None;
            public KeyCode KbRotateRight = KeyCode.None;
            public KeyCode KbSlopeLeft = KeyCode.None;
            public KeyCode KbSlopeRight = KeyCode.None;
            // ここ意味わからないけどMaidroneを真似る
            // XmlManagerにいながらxmlから読み込まないのは
            // 今は何を指定すればいいかわかっってない。
            // けど、わかったらxmlから読むように変えたいにょ
            public String JsMoveX = "Oculus_GearVR_LThumbstickX";
            public String JsMoveY = "Oculus_GearVR_LThumbstickY";

            public float MoveSpeed = 0.05f;
            public float RotateSpeed = 1.0f;

            private string xmlFileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Config\FlyingPudding.xml";
            private XmlDocument xmldoc = new XmlDocument();

            public XmlManager()
            {
                try{
                    InitXml();
                }
                catch(Exception e){
                    Debug.LogError("FlyingPudding.Plugin:" + e.Source + e.Message + e.StackTrace);
                }
            }

            private KeyCode StringToKeyCode(String keyString){
                KeyCode returnKeyCode = KeyCode.None;
                foreach (string keyName in Enum.GetNames(typeof(KeyCode)))
                {
                    if (keyString.Equals(keyName))
                    {
                        returnKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), keyString);
                        break;
                    }
                }
                return returnKeyCode;
            }

            private void InitXml()
            {
                xmldoc.Load(xmlFileName);
                // KeyConfig
                XmlNode keyConfig = xmldoc.GetElementsByTagName("KeyConfig")[0];
                KbMoveForward = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("MoveForward"));
                KbMoveBackward = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("MoveBackward"));
                KbMoveLeft = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("MoveLeft"));
                KbMoveRight = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("MoveRight"));
                KbMoveUp = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("MoveUp"));
                KbMoveDown = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("MoveDown"));
                KbRotateUp = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("RotateUp"));
                KbRotateDown = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("RotateDown"));
                KbRotateLeft = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("RotateLeft"));
                KbRotateRight = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("RotateRight"));
                KbSlopeLeft = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("SlopeLeft"));
                KbSlopeRight = StringToKeyCode(((XmlElement)keyConfig).GetAttribute("SlopeRight"));
                
                XmlNode Speed = xmldoc.GetElementsByTagName("Speed")[0];
                Single.TryParse(((XmlElement)Speed).GetAttribute("Move"), out MoveSpeed);
                Single.TryParse(((XmlElement)Speed).GetAttribute("Rotate"), out RotateSpeed);
            }


        }

    }
}


