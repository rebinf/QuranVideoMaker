using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace QuranTranslationImageGenerator
{
    public static class Quran
    {
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
            new QuranTranslation(QuranIds.EnglishSaheehInternational, Properties.Resources.en_sahih)
            {
                Language = "English",
                Name = "Saheeh International",
                Translator = "Saheeh International",
                Font = "Arial",
            },

            new QuranTranslation(QuranIds.GermanAbuRida, Properties.Resources.de_aburida)
            {
                Language = "German",
                Name = "Abu Rida",
                Translator = "Abu Rida Muhammad ibn Ahmad ibn Rassoul",
                Font = "Arial",
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
        };

        public static IEnumerable<Verse> UthmaniScript
        {
            get
            {
                return _verses;
            }
        }

        static Quran()
        {
            LoadVerses(QuranIds.Quran, _verses, Properties.Resources.quran_uthmani, true, true);
        }

        public static IEnumerable<QuranTranslation> Translations { get { return _translations; } }


        public static IEnumerable<ChapterInfo> Chapters { get { return _chapters; } }

        public static QuranTranslation GetTranslation(Guid guid)
        {
            return Translations.First(x => x.Id == guid);
        }

        public static void LoadVerses(Guid id, List<Verse> verses, string content, bool IsRightToLeft, bool isNonAscii)
        {
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
    }
}
