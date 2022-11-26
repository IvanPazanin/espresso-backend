﻿// RssFeedId.cs
//
// © 2022 Espresso News. All rights reserved.

namespace Espresso.Domain.Enums.RssFeedEnums;

/// <summary>
/// <see cref="Entities.RssFeed"/> id.
/// </summary>
public enum RssFeedId
{
#pragma warning disable SA1602 // Enumeration items should be documented
    Undefined = 0,
    Index_Vijesti = 1,
    Index_Sport = 2,
    Index_Magazin = 3,
    Index_Rogue = 4,
    Index_Auto = 5,
    DvadesetCetiriSata_Vijesti = 6,
    DvadesetCetiriSata_Show = 7,
    DvadesetCetiriSata_Sport = 8,
    DvadesetCetiriSata_Lifestyle = 9,
    DvadesetCetiriSata_Tech = 10,
    DvadesetCetiriSata_Viral = 11,
    SportskeNovosti_Sport = 12,
    JutarnjiList = 13,
    NetHr = 14,
    SlobodnaDalmacija_Novosti = 15,
    SlobodnaDalmacija_Sport = 16,
    SlobodnaDalmacija_Showbiz = 17,
    SlobodnaDalmacija_Trend = 18,
    SlobodnaDalmacija_DrustvenaMreza = 19,
    SlobodnaDalmacija_Zivot = 20,
    SlobodnaDalmacija_Zdravlje = 21,
    SlobodnaDalmacija_Moda = 22,
    SlobodnaDalmacija_Ljepota = 23,
    SlobodnaDalmacija_Putovanja = 24,
    SlobodnaDalmacija_Gastro = 25,
    SlobodnaDalmacija_Dom = 26,
    SlobodnaDalmacija_Tehnologija = 27,
    SlobodnaDalmacija_Viral = 28,
    TPortal_Vijesti = 29,
    TPortal_Biznis = 30,
    TPortal_Sport = 31,
    TPortal_Tehno = 32,
    TPortal_Showtime = 33,
    TPortal_Lifestyle = 34,
    TPortal_FunBox = 35,
    TPortal_Kultura = 36,
    VecernjiList = 37,
    Telegram = 39,
    Telegram_Telesport = 40,
    Dnevnik = 42,
    Gol_Sport = 43,
    RtlVijesti_Sport = 44,
    NewsBar_ZabavnaSatira = 46,
    NogometPlus_Nogomet = 47,
    Lider_BiznisIPolitikaHrvatska = 48,
    Lider_BiznisIPolitikaSvijet = 49,
    Lider_Trziste = 50,
    PoslovniDnevnik_VijestiHrvatska = 51,
    PoslovniDnevnik_VijestiSvijet = 52,
    PoslovniDnevnik_Kompanije = 53,
    Bug_TechVijesti = 54,
    VidiHr_TechVijesti = 55,
    Zimo_TechVijesti = 56,
    Netokracija = 57,
    PoslovniPuls = 58,
    PcChip = 59,
    Cosmopolitan = 61,
    WallHr = 62,
    LjepotaIZdravlje = 63,
    AutoNet = 64,
    N1 = 65,
    NarodHr = 66,
    Hrt_Vijesti = 67,
    Hrt_Sport = 68,
    Hrt_Magazin = 69,
    Hrt_Glazba = 70,
    StoPosto = 71,
    Dnevno = 72,
    AutomobiliHr = 73,
    DirektnoHr_Direkt = 74,
    DirektnoHr_Domovina = 75,
    DirektnoHr_EuSvijet = 76,
    DirektnoHr_Razvoj = 77,
    DirektnoHr_Sport = 78,
    DirektnoHr_Zivot = 79,
    DirektnoHr_Kolumne = 80,
    DirektnoHr_Direktno = 81,
    Scena = 82,
    DalmacijaDanas = 83,
    Nacional = 84,
    Express = 85,
    DalmacijaNews = 86,
    DalmatinskiPortal = 87,
    KulturIstra = 88,
    IPazin = 89,
    NoviList = 90,
    Parentium = 91,
    LikaKlub = 92,
    LikaExpress = 93,
    LikaOnline = 94,
    LikaPlus = 95,
    IndexHrZagreb = 96,
    ZagrebInfo = 97,
    Zagrebancija = 98,
    SjeverHr = 99,
    PrigorskiHr = 100,
    PodravinaHr = 101,
    BaranjaInfo = 102,
    GlasSlavonije = 103,
    SlavonskiHr = 104,
    IVijesti = 105,
    DubrovackiDnevnik = 106,
    IstarskaZupanija = 107,
    ZagrebOnline = 108,
    SisakInfo = 109,
    OsijekNews = 110,
    SlobodnaDalmacija_Dalmacija = 111,
    SlobodnaDalmacija_Split = 112,
    DubrovnikNet = 113,
    MakarskaDanas = 114,
    MakarskaHr = 115,
    PortalOko = 116,
    AntenaZadar = 117,
    RadioImotski = 118,
    ImotskeNovine = 119,
    PortalKastela = 120,
    HukNet = 121,
    ZadarskiList = 122,
    IstraTerraMagica = 123,
    IPress = 124,
    RijekaDanas = 125,
    Fiuman = 126,
    Riportal = 127,
    GsPress = 128,
    ZagrebackiList = 129,
    ZgPortal = 130,
    GradZagreb = 131,
    PlusRegionalniTjednik = 132,
    GlasPodravineIPrigorja = 133,
    MedimurjeInfo = 134,
    MedimurskeNovine = 135,
    ZagorjeCom = 136,
    InformativniCentarVirovitica = 137,
    NovskaIn = 138,
    NovostiHr = 139,
    Portal53 = 140,
    SbPlusHr = 141,
    PozeskaKronika = 142,
    Osijek031 = 143,
    OtvorenoHr = 144,
    GeoPolitika = 145,
    PovijestHr = 146,
    Dnevno7 = 147,
    BasketballHr = 148,
    JoomBoos = 149,
    IctBusiness = 150,
    Hcl = 151,
    ProfitirajHr = 152,
    MotoriHr = 153,
    AutoportalHr = 154,
    AutopressHr = 155,
    VozimHr = 156,
    AutoMotorSport = 157,
    Hoopster = 158,
    PrvaHnl = 159,
    AlJazeera = 160,
    HifiMedia = 161,
    GeekHr = 162,
    VizKultura = 163,
    ZivotUmjetnosti = 164,
    SvijetKulture = 165,
    GamerHr = 166,
    OgPortal = 167,
    Zupanjac = 168,
    Press032 = 169,
    BitnoNet = 170,
    PloceOnline = 171,
    KaPortalHr = 172,
    RadioMreznica = 173,
    MaxPortal = 174,
    PoslovniDnevnik = 175,
    Gp1 = 176,
    F1PulsMedia = 177,
    Racunalo = 178,
    MobHr = 179,
    StartNews = 180,
    Viral = 181,
    SportKlub = 182,
    DoktoreHitno = 183,
#pragma warning restore SA1602 // Enumeration items should be documented
}
