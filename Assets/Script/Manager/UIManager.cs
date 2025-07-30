using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using master;
public enum UIName
{
    None = 0,
    MainMenu = 1,
    UIGameplay = 2,
    UIShop = 3,
    UILose = 4,
    UISetting = 6,
    UIPause = 7,

}
public class DataUIBase
{
    public int rootIdx;
    public int topIdx;
    public string loadPath;

    public DataUIBase(int rootIdx, int topIdx, string loadPath)
    {
        this.rootIdx = rootIdx;
        this.topIdx = topIdx;
        this.loadPath = loadPath;
    }
}

public class UIManager : Singleton<UIManager>
{
    private const string PATH_UI = "UI/";
    public Transform[] rootOb;
    public UIBase[] listSceneCache;
    public RectTransform posAudioMain;
    public RectTransform posAudioInGame;
    public bool isShowClShop;
    public Canvas canvas;

    private List<UIBase> listScreenActive = new List<UIBase>();
    private Dictionary<UIName, UIBase> cacheScreen = new Dictionary<UIName, UIBase>();
    /// <summary>
    /// 0: UiController, shop, event, main, guild, free (on tab)
    /// 1: General (down top)
    /// 2: top, changeScene,login, ....
    /// 3: loading, NotiUpdate
    /// top: -1: not action || -2: Hide top || >=0: Show top
    /// </summary>
    private Dictionary<UIName, string> dir = new Dictionary<UIName, string>
        {
            // UIName, rootIdx,topIdx,loadPath
            {UIName.MainMenu,"0,1,UIMainMenu" },
            {UIName.UIGameplay,"0,1,UIGameplay" },
            {UIName.UIShop,"0,1,UIShop" },
            {UIName.UILose,"0,1,UILose" },
            {UIName.UISetting,"0,1,UISetting" },
            {UIName.UIPause,"0,1,UIPause" },
    };

    private Dictionary<UIName, DataUIBase> dic2;

    public UIName CurrentName { get; private set; }
    public bool IsAction { get; set; }
    //public UIThief thiefBoard;


    private void Start()
    {

    }


    private void Awake()
    {
        //if (CameraManager.Instance != null)
        //{
        //    canvas.worldCamera = CameraManager.Instance.cameraUI;
        //}
        dic2 = new Dictionary<UIName, DataUIBase>();
        foreach (var i in dir)
        {
            if (!dic2.ContainsKey(i.Key))
            {
                var t = i.Value.Split(',');
                dic2.Add(i.Key, new DataUIBase(int.Parse(t[0]), int.Parse(t[1]), t[2]));
            }
        }
        for (int i = 0; i < listSceneCache.Length; i++)
        {
            if (!cacheScreen.ContainsKey(listSceneCache[i].uiName))
            {
                cacheScreen.Add(listSceneCache[i].uiName, listSceneCache[i]);
            }
        }
        // if (SdkUtil.isiPad())
        // {
        //     GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
        // }
        // else
        // {
        //     GetComponent<CanvasScaler>().matchWidthOrHeight = 0f;
        // }
        IsAction = false;
    }
    public void ShowUI(UIName uIScreen, Action onHideDone = null)
    {
        ShowTop(dic2[uIScreen].topIdx);
        UIBase current = listScreenActive.Find(x => x.uiName == uIScreen);
        if (!current)
        {
            current = LoadUI(uIScreen);
            current.uiName = uIScreen;
            AddScreenActive(current, true);
        }
        current.transform.SetAsLastSibling();
        current.Show(onHideDone);
        CurrentName = uIScreen;
    }

    public T ShowUI<T>(UIName uIScreen, Action onHideDone = null) where T : UIBase
    {
        ShowTop(dic2[uIScreen].topIdx);
        UIBase current = listScreenActive.Find(x => x.uiName == uIScreen);
        if (!current)
        {
            current = LoadUI(uIScreen);
            current.uiName = uIScreen;
            AddScreenActive(current, true);
        }
        current.transform.SetAsLastSibling();
        current.Show(onHideDone);
        CurrentName = uIScreen;
        return current as T;
    }

    private void AddScreenActive(UIBase current, bool isTop)
    {
        var idx = listScreenActive.FindIndex(x => x.uiName == current.uiName);
        if (isTop)
        {
            if (idx >= 0)
            {
                listScreenActive.RemoveAt(idx);
            }
            listScreenActive.Add(current);
        }
        else
        {
            if (idx < 0)
            {
                listScreenActive.Add(current);
            }
        }
    }

    public void ShowTop(int showTopIndex = -1)
    {
        //if (showTopIndex == -1)
        //{
        //    return;
        //}
        //if (showTopIndex < -1)
        //{
        //    topUI.Hide();
        //}
        //else
        //{
        //    topUI.Show(null);
        //    topUI.ShowTopGroup(showTopIndex);
        //}
    }


    public void RefreshUI()
    {
        var idx = 0;
        while (listScreenActive.Count > idx)
        {
            listScreenActive[idx].RefreshUI();
            idx++;
        }
    }

    //private UIToast _uiToast;
    //public UIToast UIToast()
    //{
    //    if (_uiToast == null)
    //    {
    //        _uiToast = GetComponentInChildren<UIToast>();
    //    }
    //    return _uiToast;
    //}

    public T GetUI<T>(UIName uIScreen) where T : UIBase
    {
        var c = LoadUI(uIScreen);
        return c as T;
    }

    public UIBase GetUI(UIName uIScreen)
    {
        return LoadUI(uIScreen);
    }

    public UIBase GetUiActive(UIName uIScreen)
    {
        return listScreenActive.Find(x => x.uiName == uIScreen);
    }

    public T GetUiActive<T>(UIName uIScreen) where T : UIBase
    {
        var ui = listScreenActive.Find(x => x.uiName == uIScreen);
        if (ui)
        {
            return ui as T;
        }
        else
        {
            return default;
        }
    }

    private UIBase LoadUI(UIName uIScreen)
    {
        UIBase current = null;
        if (cacheScreen.ContainsKey(uIScreen))
        {
            current = cacheScreen[uIScreen];
            if (current == null)
            {
                var idx = dic2[uIScreen].rootIdx;
                current = Instantiate(Resources.Load<UIBase>(PATH_UI + dic2[uIScreen].loadPath), rootOb[idx]);
                cacheScreen[uIScreen] = current;
            }
        }
        else
        {
            var idx = dic2[uIScreen].rootIdx;
            current = Instantiate(Resources.Load<UIBase>(PATH_UI + dic2[uIScreen].loadPath), rootOb[idx]);
            cacheScreen.Add(uIScreen, current);
        }
        return current;
    }

    public void RemoveActiveUI(UIName uiName)
    {
        var idx = listScreenActive.FindIndex(x => x.uiName == uiName);
        if (idx >= 0)
        {
            var ui = listScreenActive[idx];
            listScreenActive.RemoveAt(idx);
            if (!ui.isCache && cacheScreen.ContainsKey(uiName))
            {
                cacheScreen[uiName] = null;
            }
            if (listScreenActive.Count > 0)
            {
                ShowTop(dic2[listScreenActive.Last().uiName].topIdx);
            }
        }
    }

    public void HideAll()
    {
        while (listScreenActive.Count > 0)
        {
            listScreenActive[0].Hide();
        }
        //topUI.Hide();
    }
    public void HideAllUiActive()
    {
        while (listScreenActive.Count > 0)
        {
            listScreenActive[0].Hide();
        }
    }

    public void HideAllUiActive(params UIName[] ignoreUI)
    {
        for (int i = 0; i < listScreenActive.Count; i++)
        {
            for (int j = 0; j < ignoreUI.Length; j++)
            {
                if (listScreenActive[i].uiName != ignoreUI[j])
                {
                    listScreenActive[i].Hide();
                }
            }
        }
    }

    public void HideUiActive(UIName uiName)
    {
        var ui = listScreenActive.Find(x => x.uiName == uiName);
        if (ui)
        {
            ui.Hide();
        }
    }

    public UIBase GetLastUiActive()
    {
        if (listScreenActive.Count == 0) return null;
        return listScreenActive.Last();
    }
    // public void showAdAudio(RectTransform _posAudio)
    // {
    //     AdAudioHelper.show(canvas, _posAudio);
    // }
}