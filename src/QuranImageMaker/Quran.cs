using System.Text;

namespace QuranImageMaker
{
    public static class Quran
    {
        private static char[] _arabicNumbers = new char[] { '٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩' };
        private static List<Verse> _verses = new List<Verse>();

        private static List<ChapterInfo> _chapters = new List<ChapterInfo>()
        {
            new ChapterInfo(1  , "Al-Fatihah", "the Opening", "ٱلْفَاتِحَة", 7),
            new ChapterInfo(2  , "Al-Baqarah", "the Cow", "ٱلْبَقَرَة", 286),
            new ChapterInfo(3  , "Aali Imran", "the Family of Imran", "آلِ عِمْرَان", 200),
            new ChapterInfo(4  , "An-Nisa’", "the Women", "ٱلنِّسَاء", 176),
            new ChapterInfo(5  , "Al-Ma’idah", "the Table", "ٱلْمَائِدَة", 120),
            new ChapterInfo(6  , "Al-An’am", "the Cattle", "ٱلْأَنْعَام", 165),
            new ChapterInfo(7  , "Al-A’raf", "the Heights", "ٱلْأَعْرَاف", 206),
            new ChapterInfo(8  , "Al-Anfal", "the Spoils of War", "ٱلْأَنْفَال", 75),
            new ChapterInfo(9  , "At-Taubah", "the Repentance", "ٱلتَّوْبَة", 129),
            new ChapterInfo(10 , "Yunus", "Yunus", "يُونُس", 109),
            new ChapterInfo(11 , "Hud", "Hud", "هُود", 123),
            new ChapterInfo(12 , "Yusuf", "Yusuf", "يُوسُف", 111),
            new ChapterInfo(13 , "Ar-Ra’d", "the Thunder", "ٱلرَّعْد", 43),
            new ChapterInfo(14 , "Ibrahim", "Ibrahim", "إِبْرَاهِيم", 52),
            new ChapterInfo(15 , "Al-Hijr", "the Rocky Tract", "ٱلْحِجْر", 99),
            new ChapterInfo(16 , "An-Nahl", "the Bees", "ٱلنَّحْل", 128),
            new ChapterInfo(17 , "Al-Isra’", "the Night Journey", "ٱلْإِسْرَاء", 111),
            new ChapterInfo(18 , "Al-Kahf", "the Cave", "ٱلْكَهْف", 110),
            new ChapterInfo(19 , "Maryam", "Maryam", "مَرْيَم", 98),
            new ChapterInfo(20 , "Ta-Ha", "Ta-Ha", "طه", 135),
            new ChapterInfo(21 , "Al-Anbiya’", "the Prophets", "ٱلْأَنْبِيَاء", 112),
            new ChapterInfo(22 , "Al-Haj", "the Pilgrimage", "ٱلْحَجّ", 78),
            new ChapterInfo(23 , "Al-Mu’minun", "the Believers", "ٱلْمُؤْمِنُون", 118),
            new ChapterInfo(24 , "An-Nur", "the Light", "ٱلنُّور", 64),
            new ChapterInfo(25 , "Al-Furqan", "the Criterion", "ٱلْفُرْقَان", 77),
            new ChapterInfo(26 , "Ash-Shu’ara’", "the Poets", "ٱلشُّعَرَاء", 227),
            new ChapterInfo(27 , "An-Naml", "the Ants", "ٱلنَّمْل", 93),
            new ChapterInfo(28 , "Al-Qasas", "the Stories", "ٱلْقَصَص", 88),
            new ChapterInfo(29 , "Al-Ankabut", "the Spider", "ٱلْعَنْكَبُوت", 69),
            new ChapterInfo(30 , "Ar-Rum", "the Romans", "ٱلرُّوم", 60),
            new ChapterInfo(31 , "Luqman", "Luqman", "لُقْمَان", 34),
            new ChapterInfo(32 , "As-Sajdah", "the Prostration", "ٱلسَّجْدَة", 30),
            new ChapterInfo(33 , "Al-Ahzab", "the Combined Forces", "ٱلْأَحْزَاب", 73),
            new ChapterInfo(34 , "Saba’", "Sheba", "سَبَأ", 54),
            new ChapterInfo(35 , "Al-Fatir", "the Originator", "فَاطِر", 45),
            new ChapterInfo(36 , "Ya-Sin", "Ya-Sin", "يس", 83),
            new ChapterInfo(37 , "As-Saffah", "Those Ranges in Ranks", "ٱلصَّافَّات", 182),
            new ChapterInfo(38 , "Sad", "Sad", "ص", 88),
            new ChapterInfo(39 , "Az-Zumar", "the Groups", "ٱلزُّمَر", 75),
            new ChapterInfo(40 , "Ghafar", "the Forgiver", "غَافِر", 85),
            new ChapterInfo(41 , "Fusilat", "Distinguished", "فُصِّلَت", 54),
            new ChapterInfo(42 , "Ash-Shura", "the Consultation", "ٱلشُّورىٰ", 53),
            new ChapterInfo(43 , "Az-Zukhruf", "the Gold", "ٱلْزُّخْرُف", 89),
            new ChapterInfo(44 , "Ad-Dukhan", "the Smoke", "ٱلدُّخَان", 59),
            new ChapterInfo(45 , "Al-Jathiyah", "the Kneeling", "ٱلْجَاثِيَة", 37),
            new ChapterInfo(46 , "Al-Ahqaf", "the Valley", "ٱلْأَحْقَاف", 35),
            new ChapterInfo(47 , "Muhammad", "Muhammad", "مُحَمَّد", 38),
            new ChapterInfo(48 , "Al-Fat’h", "the Victory", "ٱلْفَتْح", 29),
            new ChapterInfo(49 , "Al-Hujurat", "the Dwellings", "ٱلْحُجُرَات", 18),
            new ChapterInfo(50 , "Qaf", "Qaf", "ق", 45),
            new ChapterInfo(51 , "Adz-Dzariyah", "the Scatterers", "ٱلذَّارِيَات", 60),
            new ChapterInfo(52 , "At-Tur", "the Mount", "ٱلطُّور", 49),
            new ChapterInfo(53 , "An-Najm", "the Star", "ٱلنَّجْم", 62),
            new ChapterInfo(54 , "Al-Qamar", "the Moon", "ٱلْقَمَر", 55),
            new ChapterInfo(55 , "Ar-Rahman", "the Most Gracious", "ٱلرَّحْمَٰن", 78),
            new ChapterInfo(56 , "Al-Waqi’ah", "the Event", "ٱلْوَاقِعَة", 96),
            new ChapterInfo(57 , "Al-Hadid", "the Iron", "ٱلْحَدِيد", 29),
            new ChapterInfo(58 , "Al-Mujadilah", "The Pleading", "ٱلْمُجَادِلَة", 22),
            new ChapterInfo(59 , "Al-Hashr", "the Gathering", "ٱلْحَشْر", 24),
            new ChapterInfo(60 , "Al-Mumtahanah", "the Tested", "ٱلْمُمْتَحَنَة", 13),
            new ChapterInfo(61 , "As-Saf", "the Row", "ٱلصَّفّ", 14),
            new ChapterInfo(62 , "Al-Jum’ah", "Friday", "ٱلْجُمُعَة", 11),
            new ChapterInfo(63 , "Al-Munafiqun", "the Hypocrites", "ٱلْمُنَافِقُون", 11),
            new ChapterInfo(64 , "At-Taghabun", "the Loss & Gain", "ٱلتَّغَابُن", 18),
            new ChapterInfo(65 , "At-Talaq", "the Divorce", "ٱلطَّلَاق", 12),
            new ChapterInfo(66 , "At-Tahrim", "the Prohibition", "ٱلتَّحْرِيم", 12),
            new ChapterInfo(67 , "Al-Mulk –", "the Kingdom", "ٱلْمُلْك", 30),
            new ChapterInfo(68 , "Al-Qalam", "the Pen", "ٱلْقَلَم", 52),
            new ChapterInfo(69 , "Al-Haqqah", "the Inevitable", "ٱلْحَاقَّة", 52),
            new ChapterInfo(70 , "Al-Ma’arij", "the Elevated Passages", "ٱلْمَعَارِج", 44),
            new ChapterInfo(71 , "Nuh", "Noah", "نُوح", 28),
            new ChapterInfo(72 , "Al-Jinn", "the Jinn", "ٱلْجِنّ", 28),
            new ChapterInfo(73 , "Al-Muzammil", "the Wrapped", "ٱلْمُزَّمِّل", 20),
            new ChapterInfo(74 , "Al-Mudaththir", "the Cloaked", "ٱلْمُدَّثِّر", 56),
            new ChapterInfo(75 , "Al-Qiyamah", "the Resurrection", "ٱلْقِيَامَة", 40),
            new ChapterInfo(76 , "Al-Insan", "the Human", "ٱلْإِنْسَان", 31),
            new ChapterInfo(77 , "Al-Mursalat", "Those Sent Forth", "ٱلْمُرْسَلَات", 50),
            new ChapterInfo(78 , "An-Naba’", "the Great News", "ٱلنَّبَأ", 40),
            new ChapterInfo(79 , "An-Nazi’at", "Those Who Pull Out", "ٱلنَّازِعَات", 46),
            new ChapterInfo(80 , "‘Abasa", "He Frowned", "عَبَسَ", 42),
            new ChapterInfo(81 , "At-Takwir", "the Overthrowing", "ٱلتَّكْوِير", 29),
            new ChapterInfo(82 , "Al-Infitar", "the Cleaving", "ٱلْإِنْفِطَار", 19),
            new ChapterInfo(83 , "Al-Mutaffifin", "The Cheats", "ٱلْمُطَفِّفِين", 36),
            new ChapterInfo(84 , "Al-Inshiqaq", "the Splitting Asunder", "ٱلْإِنْشِقَاق", 25),
            new ChapterInfo(85 , "Al-Buruj", "the Stars", "ٱلْبُرُوج", 22),
            new ChapterInfo(86 , "At-Tariq", "the Nightcomer", "ٱلطَّارِق", 17),
            new ChapterInfo(87 , "Al-A’la", "the Most High", "ٱلْأَعْلَىٰ", 19),
            new ChapterInfo(88 , "Al-Ghashiyah", "the Overwhelming", "ٱلْغَاشِيَة", 26),
            new ChapterInfo(89 , "Al-Fajr", "the Dawn", "ٱلْفَجْر", 30),
            new ChapterInfo(90 , "Al-Balad", "the City", "ٱلْبَلَد", 20),
            new ChapterInfo(91 , "Ash-Shams", "the Sun", "ٱلشَّمْس", 15),
            new ChapterInfo(92 , "Al-Layl", "the Night", "ٱللَّيْل", 21),
            new ChapterInfo(93 , "Adh-Dhuha", "the Forenoon", "ٱلضُّحَىٰ", 11),
            new ChapterInfo(94 , "Al-Inshirah", "the Opening Forth", "ٱلشَّرْح", 8),
            new ChapterInfo(95 , "At-Tin", "the Fig", "ٱلتِّين", 8),
            new ChapterInfo(96 , "Al-‘Alaq", "the Clot", "ٱلْعَلَق", 19),
            new ChapterInfo(97 , "Al-Qadar", "the Night of Decree", "ٱلْقَدْر", 5),
            new ChapterInfo(98 , "Al-Bayinah", "the Proof", "ٱلْبَيِّنَة", 8),
            new ChapterInfo(99 , "Az-Zalzalah", "the Earthquake", "ٱلزَّلْزَلَة", 8),
            new ChapterInfo(100, "Al-‘Adiyah", "the Runners", "ٱلْعَادِيَات", 11),
            new ChapterInfo(101, "Al-Qari’ah", "the Striking Hour", "ٱلْقَارِعَة", 11),
            new ChapterInfo(102, "At-Takathur", "the Piling Up", "ٱلتَّكَاثُر", 8),
            new ChapterInfo(103, "Al-‘Asr", "the Time", "ٱلْعَصْر", 3),
            new ChapterInfo(104, "Al-Humazah", "the Slanderer", "ٱلْهُمَزَة", 9),
            new ChapterInfo(105, "Al-Fil", "the Elephant", "ٱلْفِيل", 5),
            new ChapterInfo(106, "Quraish", "Quraish", "قُرَيْش", 4),
            new ChapterInfo(107, "Al-Ma’un", "the Assistance", "ٱلْمَاعُون", 7),
            new ChapterInfo(108, "Al-Kauthar", "the River of Abundance", "ٱلْكَوْثَر", 3),
            new ChapterInfo(109, "Al-Kafirun", "the Disbelievers", "ٱلْكَافِرُون", 6),
            new ChapterInfo(110, "An-Nasr", "the Help", "ٱلنَّصْر", 3),
            new ChapterInfo(111, "Al-Masad", "the Palm Fiber", "ٱلْمَسَد", 5),
            new ChapterInfo(112, "Al-Ikhlas", "the Sincerity", "ٱلْإِخْلَاص", 4),
            new ChapterInfo(113, "Al-Falaq", "the Daybreak", "ٱلْفَلَق", 5),
            new ChapterInfo(114, "An-Nas", "Mankind", "ٱلنَّاس", 6),
        };

        private static List<QuranTranslation> _translations = new List<QuranTranslation>()
        {
            new QuranTranslation(QuranIds.AlbanianEfendiNahi, Properties.Resources.sq_nahi)
            {
                Language = "Albanian",
                Name = "Efendi Nahi",
                Translator = "Hasan Efendi Nahi",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.AlbanianFetiMehdiu, Properties.Resources.sq_mehdiu)
            {
                Language = "Albanian",
                Name = "Feti Mehdiu",
                Translator = "Feti Mehdiu",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.AlbanianSherifAhmeti, Properties.Resources.sq_ahmeti)
            {
                Language = "Albanian",
                Name = "Sherif Ahmeti",
                Translator = "Sherif Ahmeti",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.AmazighAtMensur, Properties.Resources.ber_mensur)
            {
                Language = "Amazigh",
                Name = "At Mensur",
                Translator = "Ramdane At Mansour",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.ArabicJalalayn, Properties.Resources.ar_jalalayn)
            {
                Language = "Arabic",
                Name = "تفسير الجلالين",
                Translator = "Jalal ad-Din al-Mahalli and Jalal ad-Din as-Suyuti",
                Font = "Amiri",
                IsRightToLeft = true,
                IsNonAscii = true
            },
            new QuranTranslation(QuranIds.ArabicAlmisr, Properties.Resources.ar_muyassar)
            {
                Language = "Arabic",
                Name = "تفسير المیسر",
                Translator = "King Fahad Quran Complex",
                Font = "Amiri",
                IsRightToLeft = true,
                IsNonAscii = true
            },
            new QuranTranslation(QuranIds.AmharicSadiqSani, Properties.Resources.am_sadiq)
            {
                Language = "Amharic",
                Name = "ሳዲቅ & ሳኒ ሐቢብ",
                Translator = "Muhammed Sadiq and Muhammed Sani Habib",
                Font = "Ebrima"
            },
            new QuranTranslation(QuranIds.AzerbaijaniVasimZiya, Properties.Resources.az_mammadaliyev)
            {
                Language = "Azerbaijani",
                Name = "Məmmədəliyev & Bünyadov",
                Translator = "Vasim Mammadaliyev and Ziya Bunyadov",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.AzerbaijaniMusayev, Properties.Resources.az_musayev)
            {
                Language = "Azerbaijani",
                Name = "Musayev",
                Translator = "Alikhan Musayev",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.BengaliZohurulHoque, Properties.Resources.bn_hoque)
            {
                Language = "Bengali",
                Name = "জহুরুল হক",
                Translator = "Zohurul Hoque",
                Font = "Nirmala UI",
            },
            new QuranTranslation(QuranIds.BengaliMuhiuddinKhan, Properties.Resources.bn_bengali)
            {
                Language = "Bengali",
                Name = "মুহিউদ্দীন খান",
                Translator = "Muhiuddin Khan",
                Font = "Nirmala UI",
            },
            new QuranTranslation(QuranIds.BosnianKorkut, Properties.Resources.bs_korkut)
            {
                Language = "Bosnian",
                Name = "Korkut",
                Translator = "Besim Korkut",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.BosnianMlivo, Properties.Resources.bs_mlivo)
            {
                Language = "Bosnian",
                Name = "Mlivo",
                Translator = "Mustafa Mlivo",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.BulgarianTzvetanTheophanov, Properties.Resources.bg_theophanov)
            {
                Language = "Bulgarian",
                Name = "Теофанов",
                Translator = "Tzvetan Theophanov",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.ChineseMaJian, Properties.Resources.zh_jian)
            {
                Language = "Chinese",
                Name = "Ma Jian",
                Translator = "Ma Jian",
                Font = "SimSun",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.ChineseMaJianTraditional, Properties.Resources.zh_majian)
            {
                Language = "Chinese",
                Name = "Ma Jian (Traditional)",
                Translator = "Ma Jian",
                Font = "SimSun",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.CzechHrbek, Properties.Resources.cs_hrbek)
            {
                Language = "Czech",
                Name = "Hrbek",
                Translator = "Preklad I. Hrbek",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.CzechNykl, Properties.Resources.cs_nykl)
            {
                Language = "Czech",
                Name = "Nykl",
                Translator = "A. R. Nykl",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.Divehi, Properties.Resources.dv_divehi)
            {
                Language = "Divehi",
                Name = "ދިވެހި",
                Translator = "Office of the President of Maldives",
                Font = "Arial",
                IsRightToLeft = true,
                IsNonAscii = true
            },
            new QuranTranslation(QuranIds.DutchKeyzer, Properties.Resources.nl_keyzer)
            {
                Language = "Dutch",
                Name = "Keyzer",
                Translator = "Salomo Keyzer",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.DutchLeemhuis, Properties.Resources.nl_leemhuis)
            {
                Language = "Dutch",
                Name = "Leemhuis",
                Translator = "Fred Leemhuis",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.DutchSiregar, Properties.Resources.nl_siregar)
            {
                Language = "Dutch",
                Name = "Siregar",
                Translator = "Sofian S. Siregar",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishAhmedAli, Properties.Resources.en_ahmedali)
            {
                Language = "English",
                Name = "Ahmed Ali",
                Translator = "Ahmed Ali",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishAhmedRazaKhan, Properties.Resources.en_ahmedraza)
            {
                Language = "English",
                Name = "Ahmed Raza Khan",
                Translator = "Ahmed Raza Khan",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishArberry, Properties.Resources.en_arberry)
            {
                Language = "English",
                Name = "Arberry",
                Translator = "A. J. Arberry",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishDaryabadi, Properties.Resources.en_daryabadi)
            {
                Language = "English",
                Name = "Daryabadi",
                Translator = "Abdul Majid Daryabadi",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishHilali, Properties.Resources.en_hilali)
            {
                Language = "English",
                Name = "Hilali & Khan",
                Translator = "Muhammad Taqi-ud-Din al-Hilali and Muhammad Muhsin Khan",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishItani, Properties.Resources.en_itani)
            {
                Language = "English",
                Name = "Itani",
                Translator = "Talal Itani",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishMaududi, Properties.Resources.en_maududi)
            {
                Language = "English",
                Name = "Maududi",
                Translator = "Abul Ala Maududi",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishMubarakpuri, Properties.Resources.en_mubarakpuri)
            {
                Language = "English",
                Name = "Mubarakpuri",
                Translator = "Safi-ur-Rahman al-Mubarakpuri",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishPickthall, Properties.Resources.en_pickthall)
            {
                Language = "English",
                Name = "Pickthall",
                Translator = "Mohammed Marmaduke William Pickthall",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishQarai, Properties.Resources.en_qarai)
            {
                Language = "English",
                Name = "Qarai",
                Translator = "Ali Quli Qarai",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishQaribullah, Properties.Resources.en_qaribullah)
            {
                Language = "English",
                Name = "Qaribullah & Darwish",
                Translator = "Hasan al-Fatih Qaribullah and Ahmad Darwish",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishSaheehInternational, Properties.Resources.en_sahih)
            {
                Language = "English",
                Name = "Saheeh International",
                Translator = "Saheeh International",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishSarwar, Properties.Resources.en_sarwar)
            {
                Language = "English",
                Name = "Sarwar",
                Translator = "Muhammad Sarwar",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishShakir, Properties.Resources.en_shakir)
            {
                Language = "English",
                Name = "Shakir",
                Translator = "Mohammad Habib Shakir",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishTransliteration, Properties.Resources.en_transliteration)
            {
                Language = "English",
                Name = "Transliteration",
                Translator = "English Transliteration",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishWahiduddin, Properties.Resources.en_wahiduddin)
            {
                Language = "English",
                Name = "Wahiduddin Khan",
                Translator = "Wahiduddin Khan",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.EnglishYusufAli, Properties.Resources.en_yusufali)
            {
                Language = "English",
                Name = "Yusuf Ali",
                Translator = "Abdullah Yusuf Ali",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.FrenchHamidullah, Properties.Resources.fr_hamidullah)
            {
                Language = "French",
                Name = "Hamidullah",
                Translator = "Muhammad Hamidullah",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.GermanBubenheimElyas, Properties.Resources.de_aburida)
            {
                Language = "German",
                Name = "Bubenheim & Elyas",
                Translator = "Abu Rida Muhammad ibn Ahmad ibn Rassoul",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.GermanAbuRida, Properties.Resources.de_bubenheim)
            {
                Language = "German",
                Name = "Abu Rida",
                Translator = "A. S. F. Bubenheim and N. Elyas",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.GermanKhoury, Properties.Resources.de_khoury)
            {
                Language = "German",
                Name = "Khoury",
                Translator = "Adel Theodor Khoury",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.GermanZaidan, Properties.Resources.de_zaidan)
            {
                Language = "German",
                Name = "Zaidan",
                Translator = "Amir Zaidan",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.HausaGumi, Properties.Resources.ha_gumi)
            {
                Language = "Hausa",
                Name = "Gumi",
                Translator = "Abubakar Mahmoud Gumi",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.HindiKhanAhmed, Properties.Resources.hi_farooq)
            {
                Language = "Hindi",
                Name = "फ़ारूक़ ख़ान & अहमद",
                Translator = "Muhammad Farooq Khan and Muhammad Ahmed",
                Font = "Nirmala UI",
            },
            new QuranTranslation(QuranIds.HindiKhanNadwi, Properties.Resources.hi_hindi)
            {
                Language = "Hindi",
                Name = "फ़ारूक़ ख़ान & नदवी",
                Translator = "Suhel Farooq Khan and Saifur Rahman Nadwi",
                Font = "Nirmala UI",
            },
            new QuranTranslation(QuranIds.IndonesianBahasa, Properties.Resources.id_indonesian)
            {
                Language = "Indonesian",
                Name = "Bahasa Indonesia",
                Translator = "Indonesian Ministry of Religious Affairs",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.IndonesianQuraishShihab, Properties.Resources.id_muntakhab)
            {
                Language = "Indonesian",
                Name = "Quraish Shihab",
                Translator = "Muhammad Quraish Shihab et al.",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.IndonesianJalalayn, Properties.Resources.id_jalalayn)
            {
                Language = "Indonesian",
                Name = "Tafsir Jalalayn",
                Translator = "Jalal ad-Din al-Mahalli and Jalal ad-Din as-Suyuti",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.ItalianPiccardo, Properties.Resources.it_piccardo)
            {
                Language = "Italian",
                Name = "Piccardo",
                Translator = "Hamza Roberto Piccardo",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.Japanese, Properties.Resources.ja_japanese)
            {
                Language = "Japanese",
                Name = "Japanese",
                Translator = "Unknown",
                Font = "SimSun",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.Korean, Properties.Resources.ko_korean)
            {
                Language = "Korean",
                Name = "Korean",
                Translator = "Unknown",
                Font = "Malgun Gothic",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.KurdishTafsiriAsan, Properties.Resources.ku_asan)
            {
                Language = "Kurdish",
                Name = "ته‌فسیری ئاسان",
                Translator = "Burhan Muhammad-Amin",
                Font = "Amiri",
                IsRightToLeft = true,
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.MalayBasmeih, Properties.Resources.ms_basmeih)
            {
                Language = "Malay",
                Name = "Basmeih",
                Translator = "Abdullah Muhammad Basmeih",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.MalayalamAbdulHameedParappoor, Properties.Resources.ml_abdulhameed)
            {
                Language = "Malayalam",
                Name = "അബ്ദുല്‍ ഹമീദ് & പറപ്പൂര്‍",
                Translator = "Cheriyamundam Abdul Hameed and Kunhi Mohammed Parappoor",
                Font = "Nirmala UI",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.MalayalamKarakunnuElayavoor, Properties.Resources.ml_karakunnu)
            {
                Language = "Malayalam",
                Name = "കാരകുന്ന് & എളയാവൂര്",
                Translator = "Muhammad Karakunnu and Vanidas Elayavoor",
                Font = "Nirmala UI",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.NorwegianEinarBerg, Properties.Resources.no_berg)
            {
                Language = "Norwegian",
                Name = "Einar Berg",
                Translator = "Einar Berg",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.PashtoAbdulwaliKhan, Properties.Resources.ps_abdulwali)
            {
                Language = "Pashto",
                Name = "عبدالولي",
                Translator = "Abdulwali Khan",
                Font = "Lateef",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianAnsarian, Properties.Resources.fa_ansarian)
            {
                Language = "Persian",
                Name = "انصاریان",
                Translator = "Hussain Ansarian",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianAbdolAyati, Properties.Resources.fa_ayati)
            {
                Language = "Persian",
                Name = "آیتی",
                Translator = "AbdolMohammad Ayati",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianBahrampour, Properties.Resources.fa_bahrampour)
            {
                Language = "Persian",
                Name = "بهرام‌پور",
                Translator = "Abolfazl Bahrampour",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianGharaati, Properties.Resources.fa_gharaati)
            {
                Language = "Persian",
                Name = "قرائتی",
                Translator = "Mohsen Gharaati",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianElahiGhomshei, Properties.Resources.fa_ghomshei)
            {
                Language = "Persian",
                Name = "الهی قمشه‌ای",
                Translator = "Mahdi Elahi Ghomshei",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianKhorramdel, Properties.Resources.fa_khorramdel)
            {
                Language = "Persian",
                Name = "خرمدل",
                Translator = "Mostafa Khorramdel",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianKhorramshahi, Properties.Resources.fa_khorramshahi)
            {
                Language = "Persian",
                Name = "خرمشاهی",
                Translator = "Baha'oddin Khorramshahi",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianSadeqiTehrani, Properties.Resources.fa_sadeqi)
            {
                Language = "Persian",
                Name = "صادقی تهرانی",
                Translator = "Mohammad Sadeqi Tehrani",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianSafavi, Properties.Resources.fa_safavi)
            {
                Language = "Persian",
                Name = "صفوی",
                Translator = "Sayyed Mohammad Reza Safavi",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianFooladvand, Properties.Resources.fa_fooladvand)
            {
                Language = "Persian",
                Name = "فولادوند",
                Translator = "Mohammad Mahdi Fooladvand",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianMojtabavi, Properties.Resources.fa_mojtabavi)
            {
                Language = "Persian",
                Name = "مجتبوی",
                Translator = "Sayyed Jalaloddin Mojtabavi",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianMoezzi, Properties.Resources.fa_moezzi)
            {
                Language = "Persian",
                Name = "معزی",
                Translator = "Mohammad Kazem Moezzi",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PersianMakaremShirazi, Properties.Resources.fa_makarem)
            {
                Language = "Persian",
                Name = "مکارم شیرازی",
                Translator = "Naser Makarem Shirazi",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.PolishBielawskiego, Properties.Resources.pl_bielawskiego)
            {
                Language = "Polish",
                Name = "Bielawskiego",
                Translator = "Józefa Bielawskiego",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.PortugueseElHayek, Properties.Resources.pt_elhayek)
            {
                Language = "Portuguese",
                Name = "El-Hayek",
                Translator = "Samir El-Hayek",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.RomanianGrigore, Properties.Resources.ro_grigore)
            {
                Language = "Romanian",
                Name = "Grigore",
                Translator = "George Grigore",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.RussianAbuAdel, Properties.Resources.ru_abuadel)
            {
                Language = "Russian",
                Name = "Абу Адель",
                Translator = "Abu Adel",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.RussianEgyptAwqaf, Properties.Resources.ru_muntahab)
            {
                Language = "Russian",
                Name = "Аль-Мунтахаб",
                Translator = "Ministry of Awqaf, Egypt",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.RussianKrachkovsky, Properties.Resources.ru_krachkovsky)
            {
                Language = "Russian",
                Name = "Крачковский",
                Translator = "Ignaty Yulianovich Krachkovsky",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.RussianKuliev, Properties.Resources.ru_kuliev)
            {
                Language = "Russian",
                Name = "Кулиев",
                Translator = "Elmir Kuliev",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.RussianKulievAbdulRahman, Properties.Resources.ru_kuliev_alsaadi)
            {
                Language = "Russian",
                Name = "Кулиев + ас-Саади",
                Translator = "Elmir Kuliev (with Abd ar-Rahman as-Saadi's commentaries)",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.RussianOsmanov, Properties.Resources.ru_osmanov)
            {
                Language = "Russian",
                Name = "Османов",
                Translator = "Magomed-Nuri Osmanovich Osmanov",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.RussianPorokhova, Properties.Resources.ru_porokhova)
            {
                Language = "Russian",
                Name = "Порохова",
                Translator = "V. Porokhova",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.RussianSablukov, Properties.Resources.ru_sablukov)
            {
                Language = "Russian",
                Name = "Саблуков",
                Translator = "Gordy Semyonovich Sablukov",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.SindhiAmroti, Properties.Resources.sd_amroti)
            {
                Language = "Sindhi",
                Name = "امروٽي",
                Translator = "Taj Mehmood Amroti",
                Font = "Arial",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.SomaliAbduh, Properties.Resources.so_abduh)
            {
                Language = "Somali",
                Name = "Abduh",
                Translator = "Mahmud Muhammad Abduh",
                Font = "Arial",
            },
            new QuranTranslation(QuranIds.SpanishBornez, Properties.Resources.es_bornez)
            {
                Language = "Spanish",
                Name = "Bornez",
                Translator = "Raúl González Bórnez",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.SpanishCortes, Properties.Resources.es_cortes)
            {
                Language = "Spanish",
                Name = "Cortes",
                Translator = "Julio Cortes",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.SpanishGarcia, Properties.Resources.es_garcia)
            {
                Language = "Spanish",
                Name = "Garcia",
                Translator = "Muhammad Isa García",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.SwahiliAlBarwani, Properties.Resources.sw_barwani)
            {
                Language = "Swahili",
                Name = "Al-Barwani",
                Translator = "Ali Muhsin Al-Barwani",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.SwedishBernstrom, Properties.Resources.sv_bernstrom)
            {
                Language = "Swedish",
                Name = "Bernström",
                Translator = "Knut Bernström",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.TajikAyati, Properties.Resources.tg_ayati)
            {
                Language = "Tajik",
                Name = "Оятӣ",
                Translator = "AbdolMohammad Ayati",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.Tamil, Properties.Resources.ta_tamil)
            {
                Language = "Tamil",
                Name = "ஜான் டிரஸ்ட்",
                Translator = "Jan Turst Foundation",
                Font = "Nirmala UI",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.Tatar, Properties.Resources.tt_nugman)
            {
                Language = "Tatar",
                Name = "Yakub Ibn Nugman",
                Translator = "Yakub Ibn Nugman",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.Thai, Properties.Resources.th_thai)
            {
                Language = "Thai",
                Name = "ภาษาไทย",
                Translator = "King Fahad Quran Complex",
                Font = "Microsoft Sans Serif",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.TurkishAbdulbaki, Properties.Resources.tr_golpinarli)
            {
                Language = "Turkish",
                Name = "Abdulbakî Gölpınarlı",
                Translator = "Abdulbakî Gölpınarlı",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.TurkishAliBulac, Properties.Resources.tr_bulac)
            {
                Language = "Turkish",
                Name = "Alİ Bulaç",
                Translator = "Alİ Bulaç",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.TurkishCeviriyazi, Properties.Resources.tr_transliteration)
            {
                Language = "Turkish",
                Name = "Çeviriyazı",
                Translator = "Muhammet Abay",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.TurkishDiyanetIsleri, Properties.Resources.tr_diyanet)
            {
                Language = "Turkish",
                Name = "Diyanet İşleri",
                Translator = "Diyanet Isleri",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.TurkishVakfi, Properties.Resources.tr_vakfi)
            {
                Language = "Turkish",
                Name = "Diyanet Vakfı",
                Translator = "Diyanet Vakfi",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.TurkishEdipYuksel, Properties.Resources.tr_yuksel)
            {
                Language = "Turkish",
                Name = "Edip Yüksel",
                Translator = "Edip Yüksel",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.TurkishElmalili, Properties.Resources.tr_yazir)
            {
                Language = "Turkish",
                Name = "Elmalılı Hamdi Yazır",
                Translator = "Elmalılı Hamdi Yazır",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.TurkishOzturk, Properties.Resources.tr_ozturk)
            {
                Language = "Turkish",
                Name = "Öztürk",
                Translator = "Yasar Nuri Ozturk",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.TurkishSuatYildirim, Properties.Resources.tr_yildirim)
            {
                Language = "Turkish",
                Name = "Suat Yıldırım",
                Translator = "Suat Yıldırım",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.TurkishSuleymanAtes, Properties.Resources.tr_ates)
            {
                Language = "Turkish",
                Name = "Süleyman Ateş",
                Translator = "Suleyman Ates",
                Font = "Arial",
                IsNonAscii = true,
            },
            new QuranTranslation(QuranIds.UrduAbulAla, Properties.Resources.ur_maududi)
            {
                Language = "Urdu",
                Name = "ابوالاعلی مودودی",
                Translator = "Abul A'ala Maududi",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.UrduAhmedRazaKhan, Properties.Resources.ur_kanzuliman)
            {
                Language = "Urdu",
                Name = "احمد رضا خان",
                Translator = "Ahmed Raza Khan",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.UrduAhmedAli, Properties.Resources.ur_ahmedali)
            {
                Language = "Urdu",
                Name = "احمد علی",
                Translator = "Ahmed Ali",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.UrduJalandhry, Properties.Resources.ur_jalandhry)
            {
                Language = "Urdu",
                Name = "جالندہری",
                Translator = "Fateh Muhammad Jalandhry",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.UrduQadri, Properties.Resources.ur_qadri)
            {
                Language = "Urdu",
                Name = "طاہر القادری",
                Translator = "Tahir ul Qadri",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.UrduJawadi, Properties.Resources.ur_jawadi)
            {
                Language = "Urdu",
                Name = "علامہ جوادی",
                Translator = "Syed Zeeshan Haider Jawadi",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.UrduJunagarhi, Properties.Resources.ur_junagarhi)
            {
                Language = "Urdu",
                Name = "محمد جوناگڑھی",
                Translator = "Muhammad Junagarhi",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.UrduNajafi, Properties.Resources.ur_najafi)
            {
                Language = "Urdu",
                Name = "محمد حسین نجفی",
                Translator = "Muhammad Hussain Najafi",
                Font = "Urdu Typesetting",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.UyghurSaleh, Properties.Resources.ug_saleh)
            {
                Language = "Uyghur",
                Name = "محمد صالح",
                Translator = "Muhammad Saleh",
                Font = "Arial",
                IsNonAscii = true,
                IsRightToLeft = true,
            },
            new QuranTranslation(QuranIds.UzbekSodik, Properties.Resources.uz_sodik)
            {
                Language = "Uzbek",
                Name = "Мухаммад Содик",
                Translator = "Muhammad Sodik Muhammad Yusuf",
                Font = "Arial",
                IsNonAscii = true,
            },
        };

        public static IEnumerable<Verse> QuranScript
        {
            get
            {
                return _verses;
            }
        }

        static Quran()
        {
            LoadQuranScript(QuranScriptType.Simple);
        }

        public static void LoadQuranScript(QuranScriptType scriptType)
        {
            var script = string.Empty;

            switch (scriptType)
            {
                case QuranScriptType.Simple:
                    script = Properties.Resources.quran_simple;
                    break;
                case QuranScriptType.SimpleClean:
                    script = Properties.Resources.quran_simple_clean;
                    break;
                case QuranScriptType.SimpleMinimal:
                    script = Properties.Resources.quran_simple_min;
                    break;
                case QuranScriptType.SimplePlain:
                    script = Properties.Resources.quran_simple_plain;
                    break;
                case QuranScriptType.Uthmani:
                    script = Properties.Resources.quran_uthmani;
                    break;
                case QuranScriptType.UthmaniMinimal:
                    script = Properties.Resources.quran_uthmani_min;
                    break;
                case QuranScriptType.UthmaniMe:
                    script = Properties.Resources.quran_uthmani_me_quran;
                    break;
                default:
                    break;
            }

            LoadVerses(QuranIds.Quran, _verses, script, true, true);
        }

        public static IEnumerable<QuranTranslation> Translations { get { return _translations; } }

        public static IEnumerable<ChapterInfo> Chapters { get { return _chapters; } }

        public static QuranTranslation GetTranslation(Guid guid)
        {
            return Translations.First(x => x.Id == guid);
        }

        public static void LoadVerses(Guid id, List<Verse> verses, string content, bool IsRightToLeft, bool isNonAscii)
        {
            verses.Clear();

            var lines = content.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !x.StartsWith("#")).ToArray();

            foreach (var line in lines)
            {
                var split = line.Split('|');
                var chapter = Convert.ToInt32(split[0]);
                var verse = Convert.ToInt32(split[1]);
                var text = split[2];

                verses.Add(new Verse(id, chapter, verse, text));
            }
        }

        public static string ToArabicNumbers(int number)
        {
            var arabicNumberString = new StringBuilder();

            foreach (var digit in number.ToString())
            {
                arabicNumberString.Append(_arabicNumbers[digit - '0']);
            }

            return arabicNumberString.ToString();
        }

        public static bool ContainsArabicNumbers(string text)
        {
            return text.Any(x => _arabicNumbers.Contains(x));
        }
    }
}
