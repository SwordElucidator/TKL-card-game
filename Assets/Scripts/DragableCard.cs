using UnityEngine;
using System.Collections;

public class DragableCard : UIDragDropItem {

    public GameObject avatorPrefeb;

    protected override void OnDragDropRelease(GameObject surface)
    {
        if (this.transform.parent.name != "My Card")
        {
            return;
        }

        base.OnDragDropRelease(surface);

        this.transform.parent.parent.Find("FightCard").GetComponent<Areas>().inlightenMap();

        if (surface != null && Areas.CanSet(this.GetComponent<Card>(), surface))
        {
            //拖拽倒了可发牌的某个区域
           GameController.skillEventsQueue.Enqueue(new NextEvent(new OneCall(this.gameObject, surface, Calls.CallSetEvent)));
           //Areas.Set(this.GetComponent<Card>(), surface);
        }
        else
        {
            transform.parent.GetComponent<MyCard>().UpdateShow();
        }
    }

    protected override void OnDragDropStart()
    {
        if (this.transform.parent.name != "My Card")
        {
            return;
        }

        base.OnDragDropStart();

        this.GetComponent<UIWidget>().width = 80;
        this.GetComponent<Card>().ResetPos();
        this.transform.parent.parent.Find("FightCard").GetComponent<Areas>().lightenSetMap(this.GetComponent<Card>());

    }
}
