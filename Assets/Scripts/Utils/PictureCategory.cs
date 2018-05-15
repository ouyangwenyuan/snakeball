using System;

[Serializable]
public class PurchaseConfig
{
    public PurchaseConfig() { }

    public string purchase_product_id;
    public NormalPayment normal_purchases;
    public int need_display_uurcahse;
    public string price;
    public int onoff;//苹果预审
    public string[] pictures;

}

[Serializable]
public class NormalPayment
{
    public NormalPayment() { }
    public WeeklyOne weeklyOne;
    public WeeklyTwo weeklyTwo;
}

[Serializable]
public class WeeklyOne
{
    public WeeklyOne() { }
    public string purchase_product_id;
    public string price;
}

[Serializable]
public class WeeklyTwo
{
    public WeeklyTwo() { }
    public string purchase_product_id;
    public string price;
}

[Serializable]
public class Category
{

    public PictureCategory[] category;
    public Category() { }
}

[Serializable]
public class PictureCategory
{

    public int id;

    public int index;

    public string name;

    public string url;

    public int ishot;

    public int isnew;
    public float price;
    public Picture[] images;
    public string duration;
    public PictureCategory()
    {

    }
}
[Serializable]
public class Picture
{

    public int id;

    public int index;

    public string name;

    public string type;

    public string url;

    public string remoteurl;

    public Picture()
    {

    }

}

