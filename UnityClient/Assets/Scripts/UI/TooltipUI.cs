using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSMOS.Core.Paterns;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace COSMOS.UI
{
    public interface ICanBeUseToShowTooltip
    {

    }
    public interface IHasName : ICanBeUseToShowTooltip
    {
        string LKeyName { get; }
    }
    public interface IHasDescription : ICanBeUseToShowTooltip
    {
        string LkeyDescription { get; }
    }
    public interface IHasIcon
    {
        string IconID { get; }
    }
    public class TooltipUI : SingletonMono<TooltipUI>
    {
        public const string TOOLTIP_PREFAB_ID = "";

        [SerializeField]
        GameObject header;
        [SerializeField]
        TextMeshProUGUI headerName;
        [SerializeField]
        Image headerImage;
        [SerializeField]
        TextMeshProUGUI description;

        private void Awake()
        {
            InitPatern();
        }

        public static TooltipUI Show(ICanBeUseToShowTooltip obj, Vector2 pos)
        {
            if(instance == null)
            {
                GameObject prefab = AssetsDatabase.LoadGameObject(TOOLTIP_PREFAB_ID);
                if(prefab == null)
                {
                    Log.Error("Tooltip prefab is null. path:" + TOOLTIP_PREFAB_ID);
                    return null;
                }
                GameObject.Instantiate(prefab);
            }
            instance.SetPosition(pos);
            instance.SetObj(obj);
            instance.gameObject.SetActive(true);
            return instance;
        }
        public void SetObj(ICanBeUseToShowTooltip obj)
        {
            if (obj != null)
            {
                if (obj is IHasName)
                {
                    lstring lname = (obj as IHasName).LKeyName;
                    headerName.SetText(lname);
                    if (obj is IHasIcon)
                    {
                        Sprite icon = AssetsDatabase.LoadSprite((obj as IHasIcon).IconID);
                        headerImage.sprite = icon;
                        headerImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        headerImage.gameObject.SetActive(false);
                    }
                    header.SetActive(true);
                }
                else
                {
                    header.SetActive(false);
                }

                if(obj is IHasDescription)
                {
                    lstring ldesc = (obj as IHasDescription).LkeyDescription;
#warning need complete description format
                    description.SetText(ldesc);
                    description.gameObject.SetActive(true);
                }
                else
                {
                    description.gameObject.SetActive(false);
                }
            }
        }
        public void SetPosition(Vector2 pos)
        {
            (gameObject.transform as RectTransform).anchoredPosition = pos;
        }
    }
}