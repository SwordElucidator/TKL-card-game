using UnityEngine;
using System.Collections;

public class StandardPackage : Package {
    
    public StandardPackage()
    {
        this.name = "StandardPackage";
        addTouhouCards();
        addKancolleCards();
        addDivasCards();
        addJKCards();
    }

    private void addTouhouCards()
    {
        CardFile Reimu = CardFile.makeCardFile(this, 6, 3, 6, 2, "博丽灵梦", "Touhou", "Reimu", TypeAge.Otome, TypeCharacter.Musessou, TypeMove.Fly);
        CardFile Youmu = CardFile.makeCardFile(this, 5, 3, 6, 1, "魂魄妖梦", "Touhou", "Youmu", TypeAge.Loli, TypeCharacter.Tekketsu, TypeMove.Walk, true);
        CardFile Yukari = CardFile.makeCardFile(this, 9, 5, 9, 2, "八云紫", "Touhou", "Yukari", TypeAge.One, TypeCharacter.Obasan, TypeMove.Fly, true);
        cards.Add(Reimu);
        cards.Add(Youmu);
        cards.Add(Yukari);
    }
    private void addKancolleCards()
    {
        CardFile Yuudachi = CardFile.makeCardFile(this, 4, 3, 5, 1, "夕立", "Kancolle", "Yuudachi", TypeAge.Loli, TypeCharacter.Kuchiku, TypeMove.Sail);
        CardFile Kaga = CardFile.makeCardFile(this, 7, 0, 9, 0, "加贺", "Kancolle", "Kaga", TypeAge.One, TypeCharacter.Wife, TypeMove.Sail);
        CardFile Akatsuki = CardFile.makeCardFile(this, 3, 2, 4, 1, "晓", "Kancolle", "Akatsuki", TypeAge.Loli, TypeCharacter.Genki, TypeMove.Sail);
        cards.Add(Yuudachi);
        cards.Add(Kaga);
        cards.Add(Akatsuki);
    }
    private void addDivasCards()
    {
        CardFile Nico = CardFile.makeCardFile(this, 6, 2, 8, 1, "矢泽妮可", "Divas", "Nico", TypeAge.Otome, TypeCharacter.BlackBelly, TypeMove.Walk);
        CardFile Ranka = CardFile.makeCardFile(this, 7, 1, 9, 1, "兰花·李", "Divas", "Ranka", TypeAge.One, TypeCharacter.Hero, TypeMove.Walk);
        CardFile Sheryl = CardFile.makeCardFile(this, 7, 2, 5, 1, "雪露·诺姆", "Divas", "Sheryl", TypeAge.Loli, TypeCharacter.Queen, TypeMove.Walk);
        cards.Add(Nico);
        cards.Add(Ranka);
        cards.Add(Sheryl);
    }
    private void addJKCards()
    {
        CardFile Ako = CardFile.makeCardFile(this, 3, 0, 4, 1, "新子憧", "JK", "Ako", TypeAge.Otome, TypeCharacter.Enkou, TypeMove.Walk);
        CardFile Kotonoha = CardFile.makeCardFile(this, 4, 4, 4, 1, "桂言叶", "JK", "Kotonoha", TypeAge.Otome, TypeCharacter.Yandere, TypeMove.Walk, true, true);
        CardFile Shana = CardFile.makeCardFile(this, 5, 3, 6, 1, "夏娜", "JK", "Shana", TypeAge.Loli, TypeCharacter.Tsundere, TypeMove.Fly, true);
        cards.Add(Ako);
        cards.Add(Kotonoha);
        cards.Add(Shana);
    }
}
