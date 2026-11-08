import os
import pptx
from pptx.util import Inches, Pt
from pptx.enum.text import PP_ALIGN, MSO_ANCHOR
from pptx.dml.color import RGBColor
from pptx.enum.shapes import MSO_SHAPE

def build_presentation():
    prs = pptx.Presentation()
    prs.slide_width = Inches(13.333)
    prs.slide_height = Inches(7.500)
    blank_layout = prs.slide_layouts[6]

    # Palette
    BG_STUDIO = RGBColor(248, 250, 252)       # Slate 50
    CARD_BG = RGBColor(255, 255, 255)         # Pure White
    CARD_BORDER = RGBColor(203, 213, 225)     # Slate 300
    TEXT_MAIN = RGBColor(15, 23, 42)          # Slate 900
    TEXT_MUTED = RGBColor(51, 65, 85)         # Slate 700
    TEXT_SUBTLE = RGBColor(100, 116, 139)     # Slate 500

    # Accents
    ACCENT_INDIGO = RGBColor(79, 70, 229)     # Indigo 600
    ACCENT_SKY = RGBColor(2, 132, 199)        # Sky 600
    ACCENT_EMERALD = RGBColor(5, 150, 105)    # Emerald 600
    ACCENT_VIOLET = RGBColor(124, 58, 237)    # Violet 600
    ACCENT_FUCHSIA = RGBColor(192, 38, 211)   # Fuchsia 600
    ACCENT_AMBER = RGBColor(217, 119, 6)      # Amber 600
    ACCENT_TEAL = RGBColor(13, 148, 136)      # Teal 600
    ACCENT_CRIMSON = RGBColor(220, 38, 38)    # Red 600

    # Code Theme
    CODE_BG = RGBColor(15, 23, 42)            # Dark Slate 900
    CODE_BORDER = RGBColor(51, 65, 85)        # Slate 700
    CODE_TEXT = RGBColor(226, 232, 240)       # Light Slate 200
    CODE_ACCENT = RGBColor(56, 189, 248)      # Sky 400
    CODE_COMMENT = RGBColor(52, 211, 153)     # Emerald 400
    CODE_OUTPUT = RGBColor(251, 191, 36)      # Amber 400

    def add_bg(slide):
        bg = slide.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(0), Inches(0), Inches(13.333), Inches(7.500))
        bg.fill.solid()
        bg.fill.fore_color.rgb = BG_STUDIO
        bg.line.fill.background()

    def add_header(slide, module_tag, title_text, slide_num, total_slides=30, accent_rgb=ACCENT_INDIGO):
        # Module tag pill
        tag_box = slide.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(0.8), Inches(0.38), Inches(3.2), Inches(0.34))
        tag_box.fill.solid()
        tag_box.fill.fore_color.rgb = accent_rgb
        tag_box.line.fill.background()
        p = tag_box.text_frame.paragraphs[0]
        p.alignment = PP_ALIGN.CENTER
        run = p.add_run()
        run.text = module_tag.upper()
        run.font.name = "Calibri"
        run.font.size = Pt(11)
        run.font.bold = True
        run.font.color.rgb = RGBColor(255, 255, 255)

        # Title
        title_box = slide.shapes.add_textbox(Inches(0.8), Inches(0.74), Inches(11.73), Inches(0.65))
        tf_title = title_box.text_frame
        tf_title.word_wrap = True
        p_title = tf_title.paragraphs[0]
        p_title.text = title_text
        p_title.font.name = "Calibri"
        p_title.font.size = Pt(25)
        p_title.font.bold = True
        p_title.font.color.rgb = TEXT_MAIN

        # Footer Left
        footer_box = slide.shapes.add_textbox(Inches(0.8), Inches(7.12), Inches(6.0), Inches(0.28))
        p_foot = footer_box.text_frame.paragraphs[0]
        p_foot.text = "101 LINQ SAMPLES · .NET 10 LTS · АРХІТЕКТУРА ТА ПАТЕРНИ"
        p_foot.font.name = "Calibri"
        p_foot.font.size = Pt(9.5)
        p_foot.font.color.rgb = TEXT_SUBTLE

        # Footer Right
        num_box = slide.shapes.add_textbox(Inches(11.53), Inches(7.12), Inches(1.0), Inches(0.28))
        p_num = num_box.text_frame.paragraphs[0]
        p_num.alignment = PP_ALIGN.RIGHT
        p_num.text = f"{slide_num:02d} / {total_slides:02d}"
        p_num.font.name = "Calibri"
        p_num.font.size = Pt(9.5)
        p_num.font.bold = True
        p_num.font.color.rgb = TEXT_SUBTLE

    def add_bottom_banner(slide, label, text, banner_type="tip"):
        top_y = 5.95
        w = 11.733
        h = 1.05
        bg_col = RGBColor(238, 242, 255) if banner_type == "tip" else RGBColor(254, 243, 199)
        border_col = ACCENT_INDIGO if banner_type == "tip" else ACCENT_AMBER
        lbl_col = ACCENT_INDIGO if banner_type == "tip" else RGBColor(180, 83, 9)

        banner = slide.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(0.8), Inches(top_y), Inches(w), Inches(h))
        banner.fill.solid()
        banner.fill.fore_color.rgb = bg_col
        banner.line.color.rgb = border_col
        banner.line.width = Pt(1.5)

        tb = slide.shapes.add_textbox(Inches(0.95), Inches(top_y + 0.08), Inches(w - 0.3), Inches(h - 0.16))
        tf = tb.text_frame
        tf.word_wrap = True
        p1 = tf.paragraphs[0]
        p1.text = label
        p1.font.name = "Calibri"
        p1.font.size = Pt(13.5)
        p1.font.bold = True
        p1.font.color.rgb = lbl_col

        p2 = tf.add_paragraph()
        p2.text = text
        p2.font.name = "Calibri"
        p2.font.size = Pt(13)
        p2.font.color.rgb = TEXT_MAIN

    def add_code_panel(slide, left, top, width, height, lines, ref_file=None):
        box = slide.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(left), Inches(top), Inches(width), Inches(height))
        box.fill.solid()
        box.fill.fore_color.rgb = CODE_BG
        box.line.color.rgb = CODE_BORDER
        box.line.width = Pt(1)

        tb = slide.shapes.add_textbox(Inches(left + 0.2), Inches(top + 0.16), Inches(width - 0.4), Inches(height - 0.32))
        tf = tb.text_frame
        tf.word_wrap = True
        tf.margin_top = Inches(0)
        tf.margin_left = Inches(0)
        tf.margin_right = Inches(0)

        for idx, l in enumerate(lines):
            p = tf.paragraphs[0] if idx == 0 else tf.add_paragraph()
            p.space_after = Pt(2)
            run = p.add_run()
            run.text = l
            run.font.name = "Consolas"
            run.font.size = Pt(11)

            if l.startswith("//"):
                run.font.color.rgb = CODE_COMMENT
            elif l.startswith("->") or l.startswith("Output:"):
                run.font.color.rgb = CODE_OUTPUT
                run.font.bold = True
            elif any(l.strip().startswith(kw) for kw in ["var", "from", "where", "select", ".Where", ".Select", ".SelectMany", ".Take", ".Skip", ".OrderBy", ".ThenBy", ".GroupBy", ".Distinct", ".Union", ".Intersect", ".Except", ".Join", ".GroupJoin", ".Count", ".Sum", ".Average", ".Aggregate", ".Any", ".All", ".First", ".Single", ".ToDictionary", ".OfType", "public", "List", "foreach", "if", "using"]):
                run.font.color.rgb = CODE_ACCENT
            else:
                run.font.color.rgb = CODE_TEXT

        if ref_file:
            p_ref = tf.add_paragraph()
            p_ref.space_before = Pt(6)
            r_ref = p_ref.add_run()
            r_ref.text = f"📍 Репозиторій: {ref_file}"
            r_ref.font.name = "Calibri"
            r_ref.font.size = Pt(10.5)
            r_ref.font.bold = True
            r_ref.font.color.rgb = CODE_ACCENT

    def add_theory_card(slide, left, top, width, height, title, title_color, bullets, sub_title=None, sub_text=None, sub_accent=ACCENT_INDIGO):
        # Outer Card
        c = slide.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(left), Inches(top), Inches(width), Inches(height))
        c.fill.solid()
        c.fill.fore_color.rgb = CARD_BG
        c.line.color.rgb = CARD_BORDER
        c.line.width = Pt(1)

        # Content Box
        content_h = 2.6 if sub_title else (height - 0.3)
        tb = slide.shapes.add_textbox(Inches(left + 0.15), Inches(top + 0.14), Inches(width - 0.3), Inches(content_h))
        tf = tb.text_frame
        tf.word_wrap = True
        p_head = tf.paragraphs[0]
        p_head.text = title
        p_head.font.name = "Calibri"
        p_head.font.size = Pt(18)
        p_head.font.bold = True
        p_head.font.color.rgb = title_color

        for b_title, b_text in bullets:
            p = tf.add_paragraph()
            p.space_before = Pt(6)
            r1 = p.add_run()
            r1.text = f"• {b_title} "
            r1.font.name = "Calibri"
            r1.font.size = Pt(13.5)
            r1.font.bold = True
            r1.font.color.rgb = TEXT_MAIN
            r2 = p.add_run()
            r2.text = b_text
            r2.font.name = "Calibri"
            r2.font.size = Pt(13)
            r2.font.color.rgb = TEXT_MUTED

        # Bottom Sub-box (fills remaining height!)
        if sub_title and sub_text:
            sub_y = top + height - 1.25
            sub = slide.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(left + 0.15), Inches(sub_y), Inches(width - 0.3), Inches(1.10))
            sub.fill.solid()
            sub.fill.fore_color.rgb = RGBColor(241, 245, 249)
            sub.line.color.rgb = RGBColor(203, 213, 225)
            sub.line.width = Pt(1)
            tf_sub = sub.text_frame
            tf_sub.word_wrap = True
            p_s1 = tf_sub.paragraphs[0]
            p_s1.text = sub_title
            p_s1.font.name = "Calibri"
            p_s1.font.size = Pt(12)
            p_s1.font.bold = True
            p_s1.font.color.rgb = sub_accent
            p_s2 = tf_sub.add_paragraph()
            p_s2.text = sub_text
            p_s2.font.name = "Calibri"
            p_s2.font.size = Pt(11.5)
            p_s2.font.color.rgb = TEXT_MAIN

    def add_dual_comparison_card(slide, left, top, width, height, title, title_color, bullets, code_lines):
        c = slide.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(left), Inches(top), Inches(width), Inches(height))
        c.fill.solid()
        c.fill.fore_color.rgb = CARD_BG
        c.line.color.rgb = CARD_BORDER
        c.line.width = Pt(1)

        tb = slide.shapes.add_textbox(Inches(left + 0.15), Inches(top + 0.14), Inches(width - 0.3), Inches(2.2))
        tf = tb.text_frame
        tf.word_wrap = True
        p_head = tf.paragraphs[0]
        p_head.text = title
        p_head.font.name = "Calibri"
        p_head.font.size = Pt(18)
        p_head.font.bold = True
        p_head.font.color.rgb = title_color

        for b_title, b_text in bullets:
            p = tf.add_paragraph()
            p.space_before = Pt(6)
            r1 = p.add_run()
            r1.text = f"• {b_title} "
            r1.font.name = "Calibri"
            r1.font.size = Pt(13.5)
            r1.font.bold = True
            r1.font.color.rgb = TEXT_MAIN
            r2 = p.add_run()
            r2.text = b_text
            r2.font.name = "Calibri"
            r2.font.size = Pt(13.5)
            r2.font.color.rgb = TEXT_MUTED

        # Bottom Code Panel inside the comparison card (fills lower half)
        add_code_panel(slide, left + 0.15, top + 2.40, width - 0.3, 1.70, code_lines)

    # ==========================================
    # SLIDE 1: HERO TITLE SLIDE
    # ==========================================
    s1 = prs.slides.add_slide(blank_layout)
    bg1 = s1.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(0), Inches(0), Inches(13.333), Inches(7.500))
    bg1.fill.solid()
    bg1.fill.fore_color.rgb = RGBColor(15, 23, 42)
    bg1.line.fill.background()

    hero_pill = s1.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(0.8), Inches(1.2), Inches(4.0), Inches(0.42))
    hero_pill.fill.solid()
    hero_pill.fill.fore_color.rgb = ACCENT_INDIGO
    hero_pill.line.fill.background()
    p_hp = hero_pill.text_frame.paragraphs[0]
    p_hp.alignment = PP_ALIGN.CENTER
    p_hp.text = ".NET 10 LTS · C# 14 · ENTERPRISE MASTERCLASS"
    p_hp.font.name = "Calibri"
    p_hp.font.size = Pt(12)
    p_hp.font.bold = True
    p_hp.font.color.rgb = RGBColor(255, 255, 255)

    tb_h1 = s1.shapes.add_textbox(Inches(0.8), Inches(1.8), Inches(11.7), Inches(2.2))
    tf_h1 = tb_h1.text_frame
    tf_h1.word_wrap = True
    p_h1 = tf_h1.paragraphs[0]
    p_h1.text = "Language Integrated Query (LINQ)\nу .NET 10"
    p_h1.font.name = "Calibri"
    p_h1.font.size = Pt(44)
    p_h1.font.bold = True
    p_h1.font.color.rgb = RGBColor(255, 255, 255)

    p_h1_sub = tf_h1.add_paragraph()
    p_h1_sub.text = "Повний курс архітектури, компіляторних механік, моделей виконання та патернів обробки даних"
    p_h1_sub.font.name = "Calibri"
    p_h1_sub.font.size = Pt(20)
    p_h1_sub.font.color.rgb = RGBColor(148, 163, 184)

    card_data = [
        ("15 КАТЕГОРІЙ", "Повний спектр операторів", "Від базової фільтрації до Hash Joins та Fold"),
        ("101 ПРИКЛАД", "100% практичного коду", "Чисті приклади на реальних моделях Northwind"),
        ("NET 10 PERFORMANCE", "Оптимізація та пам'ять", "Streaming, O(1) memory, уникнення Multiple Enumeration")
    ]
    for i, (head, sub, desc) in enumerate(card_data):
        cx = 0.8 + i * 4.0
        c = s1.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(cx), Inches(4.5), Inches(3.73), Inches(2.0))
        c.fill.solid()
        c.fill.fore_color.rgb = RGBColor(30, 41, 59)
        c.line.color.rgb = RGBColor(51, 65, 85)
        c.line.width = Pt(1)
        tf_c = c.text_frame
        tf_c.word_wrap = True
        p_c1 = tf_c.paragraphs[0]
        p_c1.text = head
        p_c1.font.name = "Calibri"
        p_c1.font.size = Pt(15)
        p_c1.font.bold = True
        p_c1.font.color.rgb = ACCENT_SKY

        p_c2 = tf_c.add_paragraph()
        p_c2.text = sub
        p_c2.font.name = "Calibri"
        p_c2.font.size = Pt(13)
        p_c2.font.bold = True
        p_c2.font.color.rgb = RGBColor(255, 255, 255)

        p_c3 = tf_c.add_paragraph()
        p_c3.text = desc
        p_c3.font.name = "Calibri"
        p_c3.font.size = Pt(11)
        p_c3.font.color.rgb = RGBColor(148, 163, 184)

    # ==========================================
    # SLIDE 2: AGENDA
    # ==========================================
    s2 = prs.slides.add_slide(blank_layout)
    add_bg(s2)
    add_header(s2, "Навчальний план", "Архітектурна карта курсу: 7 тематичних модулів", 2)

    modules_info = [
        ("01", "Архітектурний Фундамент", "Декларативність, IEnumerable / IEnumerator, автомат станів yield return, Roslyn desugaring"),
        ("02", "Механіка Виконання", "Deferred проти Immediate виконання, Streaming (O(1)) проти Buffering (O(N)), Multiple Enumeration"),
        ("03", "Фільтрація та Проєкції", "Where з індексом, Select та DTO, сплощення 1:N через SelectMany, декартовий Cross Join"),
        ("04", "Секціонування та Сортування", "Конвеєрна пагінація Skip/Take, предикатний TakeWhile/SkipWhile, IOrderedEnumerable, ThenBy"),
        ("05", "Групування та Множини", "Анатомія GroupBy, хешування ключів, ILookup кешування, Distinct, Union, Intersect, Except"),
        ("06", "Реляційні З'єднання & Агрегати", "Hash Join, GroupJoin (1:N), Left Outer Join через DefaultIfEmpty, універсальний Fold/Aggregate"),
        ("07", "Advanced & Best Practices", "Short-circuiting Any/All, First проти Single, кастомні оператори (Zip), ТОП-7 антипатернів")
    ]
    for idx, (num, m_title, m_desc) in enumerate(modules_info):
        col = 0 if idx < 4 else 1
        row = idx if col == 0 else (idx - 4)
        x = 0.8 if col == 0 else 6.81
        y = 1.55 + row * 1.32
        w = 5.72
        h = 1.20

        c = s2.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(x), Inches(y), Inches(w), Inches(h))
        c.fill.solid()
        c.fill.fore_color.rgb = CARD_BG
        c.line.color.rgb = CARD_BORDER
        c.line.width = Pt(1)

        nb = s2.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(x + 0.15), Inches(y + 0.18), Inches(0.75), Inches(0.40))
        nb.fill.solid()
        nb.fill.fore_color.rgb = ACCENT_INDIGO
        nb.line.fill.background()
        p_nb = nb.text_frame.paragraphs[0]
        p_nb.alignment = PP_ALIGN.CENTER
        p_nb.text = num
        p_nb.font.name = "Calibri"
        p_nb.font.size = Pt(14)
        p_nb.font.bold = True
        p_nb.font.color.rgb = RGBColor(255, 255, 255)

        tb = s2.shapes.add_textbox(Inches(x + 1.05), Inches(y + 0.10), Inches(w - 1.15), Inches(h - 0.20))
        tf = tb.text_frame
        tf.word_wrap = True
        p_t = tf.paragraphs[0]
        p_t.text = m_title
        p_t.font.name = "Calibri"
        p_t.font.size = Pt(14)
        p_t.font.bold = True
        p_t.font.color.rgb = TEXT_MAIN

        p_d = tf.add_paragraph()
        p_d.text = m_desc
        p_d.font.name = "Calibri"
        p_d.font.size = Pt(11)
        p_d.font.color.rgb = TEXT_MUTED

    # ==========================================
    # SLIDE 3: IMPERATIVE VS DECLARATIVE
    # ==========================================
    s3 = prs.slides.add_slide(blank_layout)
    add_bg(s3)
    add_header(s3, "01 · Архітектура", "Парадигма: Імперативний код проти Декларативного LINQ", 3)

    add_dual_comparison_card(s3, 0.8, 1.55, 5.72, 4.25,
        "🔴 Імперативний підхід («ЯК обчислювати»)", ACCENT_CRIMSON,
        [
            ("Керування потоком:", "Ручні цикли for/foreach, кроки ітерації та прапорці стану."),
            ("Виділення пам'яті:", "Обов'язкове створення проміжних списків new List<T>() у Heap."),
            ("Складність коду:", "Логіка фільтрації розпорошена серед керуючих конструкцій.")
        ],
        [
            "// ❌ Ручний перебір з виділенням пам'яті у Heap:",
            "List<Product> result = new List<Product>();",
            "foreach (var p in products)",
            "{",
            "    if (p.UnitPrice > 50 && p.UnitsInStock > 0)",
            "        result.Add(p); // Зайві алокації в пам'яті!",
            "}"
        ]
    )

    add_dual_comparison_card(s3, 6.81, 1.55, 5.72, 4.25,
        "🟢 Декларативний LINQ («ЩО отримати»)", ACCENT_EMERALD,
        [
            ("Композиція функцій:", "Опис результату через чисті предикати без сторонніх ефектів."),
            ("Потокова обробка:", "Нуль проміжних списків у пам'яті (константна пам'ять O(1))."),
            ("Безпека та якість:", "Строга типізація на етапі компіляції та самодокументований код.")
        ],
        [
            "// ✅ Декларативний потоковий конвеєр LINQ:",
            "var result = products",
            "    .Where(p => p.UnitPrice > 50 && p.UnitsInStock > 0);",
            "",
            "// Потоковий ітератор (O(1) додаткової пам'яті)!",
            "// Жодного проміжного списку не створено в Heap."
        ]
    )

    add_bottom_banner(s3, "💡 Архітектурна перевага LINQ:",
                      "Декларативність LINQ усуває понад 70% шаблонного коду перебору та виключає помилки мутабельного стану колекцій під час обробки.")

    # ==========================================
    # SLIDE 4: ROSLYN COMPILER
    # ==========================================
    s4 = prs.slides.add_slide(blank_layout)
    add_bg(s4)
    add_header(s4, "01 · Архітектура", "C# Roslyn Compiler: Децукрування та Expression Trees", 4)

    add_theory_card(s4, 0.8, 1.55, 5.72, 4.25,
        "Query Syntax проти Method Syntax", TEXT_MAIN,
        [
            ("Децукрування (Desugaring):", "Roslyn на етапі парсингу завжди перетворює Query Syntax у виклики методів розширення."),
            ("Повна еквівалентність:", "from x in list where x > 5 select x повністю транслюється в list.Where(x => x > 5)."),
            ("Байт-код IL:", "Згенерований компілятором код абсолютно ідентичний — нульова різниця у швидкодії."),
            ("Сфери застосування:", "Query Syntax краще для Join / GroupJoin; Method Syntax — для Take, Skip, OrderBy.")
        ],
        "⚡ Дві гілки абстракції LINQ:",
        "IEnumerable<T> використовує Func<T> (делегати в пам'яті).\nIQueryable<T> використовує Expression<Func<T>> (дерева виразів для SQL)."
    )

    add_code_panel(s4, 6.81, 1.55, 5.72, 4.25, [
        "// 1. Декларативний Query Syntax:",
        "var q1 = from p in products",
        "         where p.UnitsInStock > 0",
        "         select p.ProductName;",
        "",
        "// 2. Roslyn транслює це у Method Syntax:",
        "var q2 = products",
        "         .Where(p => p.UnitsInStock > 0)",
        "         .Select(p => p.ProductName);",
        "",
        "// Байт-код CIL для q1 та q2 ідентичний на 100%!",
        "-> Output: Chai, Chang, Aniseed Syrup...",
        "",
        "// Примітка: IQueryable передає Expression Tree в EF Core"
    ], "Program.cs · LinqSamplesCommon/TestData.cs")

    add_bottom_banner(s4, "⚙️ Під капотом:",
                      "IEnumerable<T> оперує скомпільованими IL-інструкціями в пам'яті. IQueryable<T> передає дерево синтаксису (Expression Tree) провайдеру EF Core для трансляції в чистий SQL.")

    # ==========================================
    # SLIDE 5: PULL MODEL & YIELD RETURN
    # ==========================================
    s5 = prs.slides.add_slide(blank_layout)
    add_bg(s5)
    add_header(s5, "01 · Архітектура", "Анатомія Ітерації: Дует IEnumerable<T> та IEnumerator<T>", 5)

    add_theory_card(s5, 0.8, 1.55, 5.72, 4.25,
        "Pull-модель та автомат станів", TEXT_MAIN,
        [
            ("Інтерфейс IEnumerable<T>:", "Є фабрикою курсорів перебору і надає єдиний метод: GetEnumerator()."),
            ("IEnumerator<T>:", "Курсор перебору зі станом: MoveNext(), Current та IDisposable."),
            ("Pull-модель (тягнуча):", "Споживач сам витягує дані викликом MoveNext(). Дані генеруються виключно за запитом."),
            ("yield return автомат:", "Roslyn створює клас-автомат станів з полем state, що пам'ятає точку зупинки.")
        ],
        "⚡ Пам'ять та ресурси:",
        "Генерація елементів через yield return вимагає O(1) додаткової пам'яті. При виході з циклу (break) викликається Dispose()."
    )

    add_code_panel(s5, 6.81, 1.55, 5.72, 4.25, [
        "// Розгортання циклу foreach компілятором Roslyn:",
        "using (IEnumerator<Product> e = products.GetEnumerator())",
        "{",
        "    while (e.MoveNext())",
        "    {",
        "        Product item = e.Current;",
        "        Console.WriteLine(item.ProductName);",
        "    }",
        "}",
        "",
        "// Якщо викликати break, виконується блок finally e.Dispose()",
        "// Наступні елементи колекції взагалі не читатимуться!"
    ], "DataSetLinq98.cs")

    add_bottom_banner(s5, "💡 Золоте правило ітератора:",
                      "Код методу з 'yield return' не виконує жодного рядка в момент виклику. Перший рядок починає виконуватися лише під час першого виклику MoveNext().")

    # ==========================================
    # SLIDE 6: DEFERRED VS IMMEDIATE
    # ==========================================
    s6 = prs.slides.add_slide(blank_layout)
    add_bg(s6)
    add_header(s6, "02 · Моделі виконання", "Відкладене (Deferred) проти Негайного (Immediate) виконання", 6, accent_rgb=ACCENT_SKY)

    add_theory_card(s6, 0.8, 1.55, 5.72, 4.25,
        "Дві моделі обчислення запитів", TEXT_MAIN,
        [
            ("Відкладене (Deferred):", "Створення запиту (Where, Select, OrderBy) не виконує фільтрацію одразу — це лише план дій."),
            ("Динамічна композиція:", "Запит можна розширювати новими Where залежно від параметрів пошуку без копіювання списків."),
            ("Негайне (Immediate):", "Оператори ToList(), ToArray(), Count(), First() миттєво матеріалізують результати в пам'ять."),
            ("Свіжість даних:", "Відкладений запит бачить зміни, внесені в колекцію ПІСЛЯ оголошення самого запиту.")
        ],
        "⚡ Точки матеріалізації (Materialization points):",
        "Будь-який термінальний виклик (.ToList, .Count, foreach) негайно запускає повний обхід конвеєра даних."
    )

    add_code_panel(s6, 6.81, 1.55, 5.72, 4.25, [
        "var list = new List<int> { 1, 2, 3 };",
        "",
        "// 1. Оголошення відкладеного запиту (нічого не обчислено!)",
        "var query = list.Where(x => x > 1);",
        "",
        "// 2. Зміна базового джерела даних у пам'яті:",
        "list.Add(4);",
        "",
        "// 3. Матеріалізація запиту споживачем:",
        "foreach (var n in query) Console.Write($\"{n} \");",
        "-> Output: 2 3 4  // Число 4 враховано автоматично!",
        "",
        "// Immediate виклик фіксує знімок стану:",
        "var snapshot = list.Where(x => x > 1).ToList();"
    ], "Linq99.cs, Linq100.cs")

    add_bottom_banner(s6, "⚠️ Увага до стану джерела:",
                      "Відкладений запит завжди звертається до актуального стану колекції на момент матеріалізації, що дозволяє створювати гнучкі фільтри без копіювання даних.")

    # ==========================================
    # SLIDE 7: STREAMING VS BUFFERING
    # ==========================================
    s7 = prs.slides.add_slide(blank_layout)
    add_bg(s7)
    add_header(s7, "02 · Моделі виконання", "Потокова обробка (Streaming) проти Буферизації (Buffering)", 7, accent_rgb=ACCENT_SKY)

    add_dual_comparison_card(s7, 0.8, 1.55, 5.72, 4.25,
        "🌊 Потокові оператори (Streaming)", ACCENT_SKY,
        [
            ("Оператори:", "Where, Select, SelectMany, Take, Skip."),
            ("Принцип дії:", "Отримує один елемент зліва, трансформує і негайно віддає направо."),
            ("Пам'ять O(1):", "Працює за фіксовану константну пам'ять незалежно від обсягу даних.")
        ],
        [
            "// Streaming: обробка потоку по 1 елементу",
            "var stream = largeFile",
            "    .Select(line => Parse(line))",
            "    .Where(record => record.IsValid);",
            "// Пам'ять залишається на рівні O(1)!"
        ]
    )

    add_dual_comparison_card(s7, 6.81, 1.55, 5.72, 4.25,
        "📦 Буферизуючі оператори (Buffering)", ACCENT_AMBER,
        [
            ("Оператори:", "OrderBy, ThenBy, GroupBy, Reverse."),
            ("Принцип дії:", "Зобов'язані вичитати 100% елементів у внутрішній буфер до видачі першого результату."),
            ("Пам'ять O(N):", "Створюють суттєве навантаження на Garbage Collector при великих обсягах.")
        ],
        [
            "// Buffering: вичитує всю колекцію в буфер!",
            "var sorted = stream.OrderBy(r => r.Timestamp);",
            "",
            "// ⚠️ Небезпека: на нескінченному потоці",
            "// виклик OrderBy спричинить OutOfMemoryException!"
        ]
    )

    add_bottom_banner(s7, "💡 Архітектурна рекомендація:",
                      "У високонавантажених пайплайнах завжди застосовуйте фільтрацію (Where) та пагінацію (Take) ПЕРЕД буферизуючими операціями (OrderBy), щоб мінімізувати розмір буфера.")

    # ==========================================
    # SLIDE 8: MULTIPLE ENUMERATION
    # ==========================================
    s8 = prs.slides.add_slide(blank_layout)
    add_bg(s8)
    add_header(s8, "02 · Моделі виконання", "Антипатерн Multiple Enumeration (Подвійна Ітерація)", 8, accent_rgb=ACCENT_SKY)

    add_theory_card(s8, 0.8, 1.55, 5.72, 4.25,
        "Анатомія прихованого витоку ресурсів", ACCENT_CRIMSON,
        [
            ("Суть проблеми:", "Один і той самий IEnumerable<T> ітерується більше одного разу (наприклад, Any() -> Count() -> foreach)."),
            ("Наслідок у БД:", "Відправляється кілька однакових важких SQL-запитів до бази даних замість одного."),
            ("Наслідок у CPU:", "Повторне виконання важких обчислень, парсинг файлів або десеріалізація JSON."),
            ("Недетермінованість:", "Якщо джерело даних встигло змінитися між ітераціями, результат буде неузгодженим.")
        ],
        "⚡ Золоте правило лікування:",
        "Якщо до послідовності звертаються більше одного разу — негайно збережіть її у List<T> або масив на вході методу."
    )

    add_code_panel(s8, 6.81, 1.55, 5.72, 4.25, [
        "// ❌ АНТИПАТЕРН: Трикратна ітерація важкого запиту",
        "public void ProcessOrders(IEnumerable<Order> orders)",
        "{",
        "    if (orders.Any())              // 1-й прохід (запит до БД)",
        "    {",
        "        var count = orders.Count();// 2-й прохід (запит до БД)",
        "        foreach (var o in orders)  // 3-й прохід (запит до БД)",
        "            Process(o);",
        "    }",
        "}",
        "",
        "// ✅ ПРАВИЛЬНО: Матеріалізація на вході",
        "var orderList = orders.ToList();   // Лише 1 запит до БД!"
    ], "Linq101.cs")

    add_bottom_banner(s8, "⚠️ Золоте правило безпеки:",
                      "Якщо до вхідного параметра IEnumerable<T> звертаються більше ніж один раз — негайно збережіть його у List<T> або масив для усунення зайвих запитів.", banner_type="warn")

    # ==========================================
    # SLIDE 9: WHERE & INDEXED WHERE
    # ==========================================
    s9 = prs.slides.add_slide(blank_layout)
    add_bg(s9)
    add_header(s9, "03 · Фільтрація та проєкції", "Фільтрація за Предикатом: Where та Індексований Where", 9, accent_rgb=ACCENT_EMERALD)

    add_theory_card(s9, 0.8, 1.55, 5.72, 4.25,
        "Принцип роботи оператора Where", TEXT_MAIN,
        [
            ("Базовий Where:", "Приймає Func<T, bool> та просіює потік за O(1) додаткової пам'яті."),
            ("Індексований Where:", "Перевантаження Func<T, int, bool> передає порядковий номер елемента (0-based) без зовнішніх лічильників."),
            ("Усунення стану:", "Більше немає потреби заводити зовнішній лічильник int i = 0 перед циклом перебору."),
            ("Композиція предикатів:", "Where(p1).Where(p2) об'єднується компілятором у Where(x => p1(x) && p2(x)).")
        ],
        "⚡ Алгоритмічна характеристика:",
        "Часова складність: O(N) в один прохід. Просторова складність: O(1) (чистий потоковий ітератор)."
    )

    add_code_panel(s9, 6.81, 1.55, 5.72, 4.25, [
        "// Приклад 1: Базовий Where (відбір товарів без залишку)",
        "var outOfStock = products.Where(p => p.UnitsInStock == 0);",
        "",
        "// Приклад 2: Індексований Where (елементи менші за свій індекс)",
        "int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };",
        "var shortDigits = numbers.Where((num, index) => num < index);",
        "",
        "foreach (var n in shortDigits) Console.Write($\"{n} \");",
        "-> Output: 1 2 0",
        "// Пояснення: 1 < index(2), 2 < index(8), 0 < index(9)"
    ], "RestrictionOperators.cs (Linq1 - Linq5)")

    add_bottom_banner(s9, "💡 Чистий функціональний код:",
                      "Індексований Where усуває мутабельний стан і дає змогу реалізувати складні алгоритми вибірки парних позицій чи вибірок за кроком (stride) в один рядок.")

    # ==========================================
    # SLIDE 10: SELECT & DTO PROJECTIONS
    # ==========================================
    s10 = prs.slides.add_slide(blank_layout)
    add_bg(s10)
    add_header(s10, "03 · Фільтрація та проєкції", "Проєкція Даних: Select, Кортежі та Легковажні DTO", 10, accent_rgb=ACCENT_EMERALD)

    add_theory_card(s10, 0.8, 1.55, 5.72, 4.25,
        "Трансформація типу 1:1", TEXT_MAIN,
        [
            ("Оператор Select:", "Перетворює кожен елемент типу TSource на TResult (відображення f: X -> Y)."),
            ("Патерн DTO:", "Захищає від витоку внутрішніх полів доменної моделі назовні (API, UI контракти)."),
            ("Анонімні типи:", "new { p.ProductName, p.UnitPrice } створює незмінний типізований об'єкт без нового класу."),
            ("Кортежі ValueTuple:", "(p.ProductName, p.UnitPrice) — розміщуються на стеку, уникаючи виділення у Heap.")
        ],
        "⚡ Оптимізація бази даних у EF Core:",
        "У роботі з базою даних проєкція Select транслюється в конкретний SELECT col1, col2, не вивантажуючи зайві поля."
    )

    add_code_panel(s10, 6.81, 1.55, 5.72, 4.25, [
        "// Проєкція у легковажний DTO з розрахунковим полем:",
        "var productDtos = products.Select(p => new ProductDto",
        "{",
        "    Id = p.ProductID,",
        "    Name = p.ProductName,",
        "    TotalPrice = p.UnitPrice * p.UnitsInStock",
        "});",
        "",
        "// Індексований Select (нумерація списку на льоту):",
        "var numbered = words.Select((w, idx) => $\"{idx + 1}. {w}\");",
        "-> Output: 1. Apple | 2. Banana | 3. Cherry"
    ], "ProjectionOperators.cs (Linq6 - Linq13)")

    add_bottom_banner(s10, "💡 Практика Clean Architecture:",
                      "Проєкція сутностей бази даних у контракти запиту (DTO) всередині Select є базовим патерном розробки високонавантажених REST API та Webhook-сервісів.")

    # ==========================================
    # SLIDE 11: SELECTMANY FLATTENING
    # ==========================================
    s11 = prs.slides.add_slide(blank_layout)
    add_bg(s11)
    add_header(s11, "03 · Фільтрація та проєкції", "Патерн 1:N — Сплощення Колекцій через SelectMany", 11, accent_rgb=ACCENT_EMERALD)

    add_theory_card(s11, 0.8, 1.55, 5.72, 4.25,
        "Сплощення деревоподібних структур", TEXT_MAIN,
        [
            ("Проблема вкладеності:", "Select(c => c.Orders) повертає IEnumerable<List<Order>> (список списків)."),
            ("Рішення SelectMany:", "Розгортає кожен вкладений список і зшиває їх в один плаский потік IEnumerable<Order>."),
            ("Функціональний аналог:", "Оператор FlatMap у мовах Java, JavaScript, Rust або зв'язувач Bind (>>=) у Haskell."),
            ("Батьківський контекст:", "Перевантаження resultSelector зберігає доступ і до батька, і до дочірнього елемента.")
        ],
        "⚡ Монадний патерн FlatMap:",
        "SelectMany дозволяє обробляти складні зв'язки «один-до-багатьох» як єдиний лінійний потік даних без циклів."
    )

    add_code_panel(s11, 6.81, 1.55, 5.72, 4.25, [
        "// Витягування всіх замовлень клієнтів з Вашингтона:",
        "var waOrders =",
        "    customers",
        "        .Where(c => c.Region == \"WA\")",
        "        .SelectMany(c => c.Orders, (c, o) => new",
        "        {",
        "            c.CustomerID,",
        "            o.OrderID,",
        "            o.OrderDate",
        "        });",
        "",
        "-> На виході: єдиний лінійний потік замовлень",
        "// Без вкладених списків і без зайвих циклів!"
    ], "ProjectionOperators.cs (Linq14 - Linq17)")

    add_bottom_banner(s11, "💡 Монадний патерн FlatMap:",
                      "SelectMany дає змогу розгортати ієрархічні гранулярні структури даних (Категорія -> Товар -> Відгук) у єдиний потік для подальшої фільтрації та аналізу.")

    # ==========================================
    # SLIDE 12: SELECTMANY CROSS JOIN
    # ==========================================
    s12 = prs.slides.add_slide(blank_layout)
    add_bg(s12)
    add_header(s12, "03 · Фільтрація та проєкції", "SelectMany як Декартовий Добуток (Cross Join)", 12, accent_rgb=ACCENT_EMERALD)

    add_theory_card(s12, 0.8, 1.55, 5.72, 4.25,
        "Комбінаторика без реляційних ключів", TEXT_MAIN,
        [
            ("Декартовий добуток:", "Генерація пар кожного елемента множини А з кожним елементом множини Б (N * M пар)."),
            ("Синтаксис запитів:", "Два оператори from поспіль (from a in seqA from b in seqB) автоматично стають SelectMany."),
            ("Фільтрація на льоту:", "Додавання where між from відсікає зайві пари без генерації повної декартової матриці."),
            ("Сценарії застосування:", "Координатні сітки, комбінації параметрів для тестів, перебір варіантів цін.")
        ],
        "⚡ Декартовий добуток у комбінаториці:",
        "SelectMany усуває необхідність писати вкладені цикли для створення декартових пар N × M елементів."
    )

    add_code_panel(s12, 6.81, 1.55, 5.72, 4.25, [
        "int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };",
        "int[] numbersB = { 1, 3, 5, 7, 8 };",
        "",
        "// Пари, де число з масиву А менше за число з масиву Б:",
        "var pairs =",
        "    from a in numbersA",
        "    from b in numbersB",
        "    where a < b",
        "    select new { A = a, B = b };",
        "",
        "foreach (var pair in pairs) Console.WriteLine($\"{pair.A} < {pair.B}\");",
        "-> Output: 0 < 1 | 0 < 3 ... 4 < 5 | 4 < 7 | 4 < 8"
    ], "ProjectionOperators.cs (Linq18 - Linq19)")

    add_bottom_banner(s12, "⚙️ Компіляторне перетворення:",
                      "Запис з двома 'from' у Query Syntax є найбільш природним та лаконічним виразом декартового добутку в сучасній мові C#.")

    # ==========================================
    # SLIDE 13: PIPELINE PAGING
    # ==========================================
    s13 = prs.slides.add_slide(blank_layout)
    add_bg(s13)
    add_header(s13, "04 · Секціонування та сортування", "Конвеєрна Пагінація: Take, Skip та C# Index/Range", 13, accent_rgb=ACCENT_VIOLET)

    add_theory_card(s13, 0.8, 1.55, 5.72, 4.25,
        "Секціонування потоків даних", TEXT_MAIN,
        [
            ("Take(n):", "Повертає перші n елементів. Після цього ітерація негайно завершується (Short-circuit)."),
            ("Skip(n):", "Пропускає перші n елементів та повертає всі наступні."),
            ("Формула пагінації:", "source.Skip((page - 1) * pageSize).Take(pageSize)."),
            ("Синтаксис діапазонів:", "Take(..5) (перші 5 елементів) або Take(^3..) (останні 3 елементи).")
        ],
        "⚡ Пакетування .NET 6+ Chunk(size):",
        "Новий метод Chunk(50) автоматично ділить великий потік на масиви для batch-операцій у базі даних."
    )

    add_code_panel(s13, 6.81, 1.55, 5.72, 4.25, [
        "int page = 3;      // Номер сторінки",
        "int pageSize = 10; // Розмір сторінки",
        "",
        "// Отримання 3-ї сторінки каталогу клієнтів Вашингтона:",
        "var pagedCustomers =",
        "    customers",
        "        .Where(c => c.Region == \"WA\")",
        "        .OrderBy(c => c.CustomerID) // Обов'язкове сортування!",
        "        .Skip((page - 1) * pageSize)",
        "        .Take(pageSize);",
        "",
        "// .NET 6+ пакетування для масових операцій:",
        "foreach (var batch in products.Chunk(50))",
        "    await BulkInsertAsync(batch);"
    ], "PartitioningOperators.cs (Linq20 - Linq23)")

    add_bottom_banner(s13, "⚠️ Критична вимога пагінації:",
                      "Пагінація через Skip/Take ЗАВЖДИ вимагає детермінованого сортування OrderBy перед секціонуванням, інакше порядок рядків на сторінках буде плаваючим.", banner_type="warn")

    # ==========================================
    # SLIDE 14: TAKEWHILE VS SKIPWHILE
    # ==========================================
    s14 = prs.slides.add_slide(blank_layout)
    add_bg(s14)
    add_header(s14, "04 · Секціонування та сортування", "Предикатне Секціонування: TakeWhile проти SkipWhile", 14, accent_rgb=ACCENT_VIOLET)

    add_dual_comparison_card(s14, 0.8, 1.55, 5.72, 4.25,
        "🛑 Оператор TakeWhile", ACCENT_VIOLET,
        [
            ("Принцип дії:", "Читає елементи, доки умова істинна (true)."),
            ("Зупинка конвеєра:", "На першому ж елементі з false — миттєве припинення роботи."),
            ("Швидкість:", "Не сканує всю колекцію, колосальний виграш на відсортованих даних.")
        ],
        [
            "int[] numbers = { 2, 4, 6, 1, 8, 10 };",
            "// Зупиниться на числі 1!",
            "var tw = numbers.TakeWhile(n => n % 2 == 0);",
            "-> Output: 2 4 6",
            "// Числа 8 та 10 навіть не перевірялися!"
        ]
    )

    add_dual_comparison_card(s14, 6.81, 1.55, 5.72, 4.25,
        "⏩ Оператор SkipWhile", ACCENT_INDIGO,
        [
            ("Принцип дії:", "Ігнорує елементи, доки умова істинна (true)."),
            ("Відкриття потоку:", "На першому ж елементі з false видає цей і ВСІ наступні елементи."),
            ("Без повторної перевірки:", "Після першого false умова більше не перевіряється.")
        ],
        [
            "int[] numbers = { 2, 4, 6, 1, 8, 10 };",
            "// Пропустить 2, 4, 6 і видасть решту:",
            "var sw = numbers.SkipWhile(n => n % 2 == 0);",
            "-> Output: 1 8 10",
            "// Видав 1, а далі 8 і 10 без перевірки!"
        ]
    )

    add_bottom_banner(s14, "💡 Продуктивність у часових рядах:",
                      "У впорядкованих за часом або ключем послідовностях TakeWhile дозволяє відсікати застарілі дані за O(1) часу без перебору мільйонів наступних записів.")

    # ==========================================
    # SLIDE 15: ORDERBY & THENBY
    # ==========================================
    s15 = prs.slides.add_slide(blank_layout)
    add_bg(s15)
    add_header(s15, "04 · Секціонування та сортування", "Багатовимірне Впорядкування: OrderBy та ThenBy", 15, accent_rgb=ACCENT_VIOLET)

    add_theory_card(s15, 0.8, 1.55, 5.72, 4.25,
        "Інтерфейс IOrderedEnumerable<T>", TEXT_MAIN,
        [
            ("IOrderedEnumerable<T>:", "OrderBy повертає спеціалізований тип, що зберігає первинний ключ сортування."),
            ("Фатальна помилка:", "list.OrderBy(x).OrderBy(y) — друге сортування ПОВНІСТЮ СКАСОВУЄ перше!"),
            ("Вторинне сортування ThenBy:", "Створює композитний компаратор для елементів з однаковим першим ключем."),
            ("Стабільність сортування:", "Алгоритм є стабільним (зберігає вихідний порядок рівних елементів).")
        ],
        "⚡ Архітектурне правило:",
        "У будь-якому конвеєрі метод OrderBy викликається рівно один раз. Усі наступні рівні впорядкування задаються виключно через ThenBy."
    )

    add_code_panel(s15, 6.81, 1.55, 5.72, 4.25, [
        "// ❌ НЕПРАВИЛЬНО: Подвійний OrderBy скидає сортування!",
        "var bad = products",
        "    .OrderBy(p => p.Category)",
        "    .OrderBy(p => p.UnitPrice); // Зламало категорію!",
        "",
        "// ✅ ПРАВИЛЬНО: Композитне сортування через ThenBy:",
        "var sortedProducts =",
        "    products",
        "        .OrderBy(p => p.Category)",
        "        .ThenByDescending(p => p.UnitPrice)",
        "        .ThenBy(p => p.ProductName);",
        "",
        "// Забезпечує детермінований порядок у звітах і таблицях"
    ], "OrderingOperators.cs (Linq28 - Linq35)")

    add_bottom_banner(s15, "⚠️ Архітектурний стандарт:",
                      "Використовуйте OrderBy лише один раз на початку ланцюжка, для всіх наступних полів — виключно ThenBy або ThenByDescending.", banner_type="warn")

    # ==========================================
    # SLIDE 16: CUSTOM COMPARERS
    # ==========================================
    s16 = prs.slides.add_slide(blank_layout)
    add_bg(s16)
    add_header(s16, "04 · Секціонування та сортування", "Кастомні Компаратори та Розворот: IComparer<T> і Reverse", 16, accent_rgb=ACCENT_VIOLET)

    col_w = 3.73
    col_configs = [
        ("IComparer<T>", "Сортування за довжиною", [
            "Реалізація IComparer<string>.",
            "x.Length.CompareTo(y.Length).",
            "Ізоляція логіки порівняння."
        ], [
            "class LengthComp : IComparer<string> {",
            "  public int Compare(string x, string y)",
            "    => x.Length.CompareTo(y.Length);",
            "}"
        ]),
        ("StringComparer", "Регістронезалежність", [
            "Готові оптимізовані компаратори.",
            "StringComparer.OrdinalIgnoreCase.",
            "Усунення помилок локалі."
        ], [
            "var sorted = words.OrderBy(",
            "    w => w,",
            "    StringComparer.OrdinalIgnoreCase",
            ");"
        ]),
        ("Reverse()", "Інверсія послідовності", [
            "Буферизує вхідний потік.",
            "Видає елементи у зворотному порядку.",
            "O(N) додаткової пам'яті."
        ], [
            "int[] digits = { 1, 2, 3, 4, 5 };",
            "var rev = digits.Reverse();",
            "-> Output: 5 4 3 2 1"
        ])
    ]

    for idx, (head, sub, items, code_lines) in enumerate(col_configs):
        cx = 0.8 + idx * 4.0
        c = s16.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(cx), Inches(1.55), Inches(col_w), Inches(4.25))
        c.fill.solid()
        c.fill.fore_color.rgb = CARD_BG
        c.line.color.rgb = CARD_BORDER
        c.line.width = Pt(1)

        tb = s16.shapes.add_textbox(Inches(cx + 0.15), Inches(1.68), Inches(col_w - 0.3), Inches(2.1))
        tf = tb.text_frame
        tf.word_wrap = True
        p1 = tf.paragraphs[0]
        p1.text = head
        p1.font.name = "Calibri"
        p1.font.size = Pt(16)
        p1.font.bold = True
        p1.font.color.rgb = ACCENT_VIOLET

        p2 = tf.add_paragraph()
        p2.text = sub
        p2.font.name = "Calibri"
        p2.font.size = Pt(13)
        p2.font.bold = True
        p2.font.color.rgb = TEXT_MAIN

        for item in items:
            pi = tf.add_paragraph()
            pi.text = f"• {item}"
            pi.font.name = "Calibri"
            pi.font.size = Pt(12)
            pi.font.color.rgb = TEXT_MUTED

        add_code_panel(s16, cx + 0.15, 3.95, col_w - 0.3, 1.70, code_lines)

    add_bottom_banner(s16, "📍 Практика в репозиторії:",
                      "У прикладі Linq36 (OrderingOperators.cs) реалізовано CaseInsensitiveComparer, який усуває вплив верхнього/нижнього регістра без створення копій ToLower().")

    # ==========================================
    # SLIDE 17: GROUPBY ANATOMY
    # ==========================================
    s17 = prs.slides.add_slide(blank_layout)
    add_bg(s17)
    add_header(s17, "05 · Групування та множини", "Групування Даних: Анатомія Оператора GroupBy", 17, accent_rgb=ACCENT_FUCHSIA)

    add_theory_card(s17, 0.8, 1.55, 5.72, 4.25,
        "Інтерфейс IGrouping<TKey, TElement>", TEXT_MAIN,
        [
            ("Кожна група — це колекція:", "Результат GroupBy — це IEnumerable<IGrouping<TKey, TElement>>, де кожна група містить властивість .Key."),
            ("Хеш-таблиця в пам'яті:", "Будує внутрішню таблицю (LookupTable), розбиваючи вхідний потік на кошики за хешем ключа."),
            ("Обов'язкова буферизація:", "GroupBy зобов'язаний вичитати всі елементи, перш ніж видати першу групу."),
            ("Селектори key та element:", "Дозволяє одночасно групувати за однією властивістю, а зберігати інше поле.")
        ],
        "⚡ Вимоги до ключів групування:",
        "Ключ зобов'язаний коректно реалізовувати GetHashCode() та Equals(). Групування за примітивами (int, Guid) працює найшвидше."
    )

    add_code_panel(s17, 6.81, 1.55, 5.72, 4.25, [
        "// Групування товарів за категорією з вибіркою назв:",
        "var productGroups =",
        "    products.GroupBy(",
        "        p => p.Category,        // KeySelector",
        "        p => p.ProductName      // ElementSelector",
        "    );",
        "",
        "foreach (var group in productGroups)",
        "{",
        "    Console.WriteLine($\"Категорія: {group.Key}\");",
        "    foreach (var name in group)",
        "        Console.WriteLine($\"  - {name}\");",
        "}",
        "-> На виході: деревоподібна структура звітів"
    ], "GroupingOperators.cs (Linq41 - Linq45)")

    add_bottom_banner(s17, "💡 Продуктивність ключів групування:",
                      "Уникайте групування за складними довгими рядками або нетипізованими об'єктами. Групування за примітивними типами працює в рази швидше.")

    # ==========================================
    # SLIDE 18: TOLOOKUP
    # ==========================================
    s18 = prs.slides.add_slide(blank_layout)
    add_bg(s18)
    add_header(s18, "05 · Групування та множини", "Багатозначні Словники: ILookup<TKey, TElement>", 18, accent_rgb=ACCENT_FUCHSIA)

    add_theory_card(s18, 0.8, 1.55, 5.72, 4.25,
        "GroupBy проти ToLookup()", TEXT_MAIN,
        [
            ("Лінивий проти негайного:", "GroupBy ліниво генерує групи; ToLookup() негайно матеріалізує незмінний індекс 1:N у пам'яті."),
            ("Безпека пошуку за O(1):", "Якщо ключа немає, lookup[key] повертає порожню послідовність Enumerable.Empty<T>(), а не KeyNotFoundException!"),
            ("Ідеально для кешування:", "Дозволяє одноразово завантажити всі довідники і виконувати швидкий пошук за ключем."),
            ("Незмінність (Immutability):", "ILookup є read-only структурою, захищеною від випадкової модифікації.")
        ],
        "⚡ Патерн кешування в пам'яті:",
        "ToLookup — ідеальна структура для швидкого багаторазового кешованого пошуку списків сутностей за зовнішнім ідентифікатором."
    )

    add_code_panel(s18, 6.81, 1.55, 5.72, 4.25, [
        "// Одноразова побудова багатозначного індексу замовлень:",
        "ILookup<string, Order> ordersByCustomer =",
        "    orders.ToLookup(o => o.CustomerID);",
        "",
        "// Миттєвий безпечний пошук за O(1) часу:",
        "var alkiOrders = ordersByCustomer[\"ALFKI\"];",
        "Console.WriteLine($\"Замовлень: {alkiOrders.Count()}\");",
        "",
        "// Якщо клієнта немає — повертається порожній список:",
        "var unknown = ordersByCustomer[\"NOT_EXISTS\"];",
        "-> Output: 0 (жодного KeyNotFoundException!)"
    ], "GroupingOperators.cs (Linq46 - Linq48)")

    add_bottom_banner(s18, "💡 Патерн кешування в пам'яті:",
                      "ToLookup є ідеальним рішенням для організації швидкого індексованого пошуку пов'язаних сутностей у пам'яті без звернень до зовнішніх баз даних.")

    # ==========================================
    # SLIDE 19: DISTINCT & UNION
    # ==========================================
    s19 = prs.slides.add_slide(blank_layout)
    add_bg(s19)
    add_header(s19, "05 · Групування та множини", "Теоретико-Множинні Операції: Distinct та Union", 19, accent_rgb=ACCENT_FUCHSIA)

    add_dual_comparison_card(s19, 0.8, 1.55, 5.72, 4.25,
        "✨ Оператор Distinct", ACCENT_FUCHSIA,
        [
            ("Усунення дублікатів:", "Пропускає лише першу появу кожного унікального значення."),
            ("Хеш-множина Set<T>:", "Використовує внутрішній Set для O(1) перевірки наявності."),
            ("Нововведення .NET 6+:", "DistinctBy(p => p.Id) для унікалізації за конкретним полем.")
        ],
        [
            "int[] factors = { 2, 2, 3, 5, 5 };",
            "var unique = factors.Distinct();",
            "-> Output: 2 3 5",
            "",
            "// Унікалізація об'єктів за полем (.NET 6+):",
            "var onePerCat = products.DistinctBy(p => p.Category);"
        ]
    )

    add_dual_comparison_card(s19, 6.81, 1.55, 5.72, 4.25,
        "🔗 Оператор Union", ACCENT_INDIGO,
        [
            ("Об'єднання множин:", "Склеює дві послідовності без повторення дублікатів."),
            ("Відмінність від Concat:", "Concat залишає дублікати; Union гарантує унікальність."),
            ("Складність O(N + M):", "Лінійний час обходу обох колекцій з побудовою хеш-множини.")
        ],
        [
            "int[] seqA = { 1, 2, 3 };",
            "int[] seqB = { 2, 3, 4 };",
            "var res = seqA.Union(seqB);",
            "-> Output: 1 2 3 4",
            "",
            "// Concat дав би: 1 2 3 2 3 4 (з дублікатами)"
        ]
    )

    add_bottom_banner(s19, "💡 Алгоритмічна складність:",
                      "Distinct та Union мають лінійну складність O(N + M) за часом і пам'яттю. Завжди забезпечуйте наявність якісної хеш-функції GetHashCode у порівнюваних об'єктів.")

    # ==========================================
    # SLIDE 20: INTERSECT & EXCEPT
    # ==========================================
    s20 = prs.slides.add_slide(blank_layout)
    add_bg(s20)
    add_header(s20, "05 · Групування та множини", "Перетин та Різниця Множин: Intersect та Except", 20, accent_rgb=ACCENT_FUCHSIA)

    add_dual_comparison_card(s20, 0.8, 1.55, 5.72, 4.25,
        "🎯 Оператор Intersect (Перетин)", ACCENT_FUCHSIA,
        [
            ("Спільні елементи:", "Повертає значення, присутні в ОБОХ колекціях одночасно."),
            ("Унікальність результату:", "Автоматично видаляє дублікати з фінального результату."),
            ("Нововведення .NET 6+:", "IntersectBy дозволяє порівнювати за зовнішнім ключем.")
        ],
        [
            "int[] a = { 0, 2, 4, 5, 6, 8 };",
            "int[] b = { 1, 3, 5, 7, 8 };",
            "var common = a.Intersect(b);",
            "-> Output: 5 8",
            "// Тільки спільні числа обох масивів!"
        ]
    )

    add_dual_comparison_card(s20, 6.81, 1.55, 5.72, 4.25,
        "✂️ Оператор Except (Різниця A \\ B)", ACCENT_CRIMSON,
        [
            ("Різниця множин:", "Повертає елементи з А, яких взагалі НЕМАЄ у множині Б."),
            ("Пошук дельти синхронізації:", "Ідеально для виявлення видалених або відсутніх записів."),
            ("Порядок має значення:", "A.Except(B) не дорівнює B.Except(A).")
        ],
        [
            "int[] a = { 0, 2, 4, 5, 6, 8 };",
            "int[] b = { 1, 3, 5, 7, 8 };",
            "var diff = a.Except(b);",
            "-> Output: 0 2 4 6",
            "",
            "// Пошук клієнтів без замовлень:",
            "var inactive = allCustIds.Except(orderCustIds);"
        ]
    )

    add_bottom_banner(s20, "💡 Патерн синхронізації:",
                      "Оператор Except — це найшвидший спосіб знайти різницю між локальним кешем та серверними даними для формування batch-запитів на оновлення.")

    # ==========================================
    # SLIDE 21: INNER JOIN
    # ==========================================
    s21 = prs.slides.add_slide(blank_layout)
    add_bg(s21)
    add_header(s21, "06 · Реляційні з'єднання", "Внутрішнє Реляційне З'єднання (Inner Join)", 21, accent_rgb=ACCENT_AMBER)

    add_theory_card(s21, 0.8, 1.55, 5.72, 4.25,
        "Алгоритм Hash Join у LINQ", TEXT_MAIN,
        [
            ("Реляційне поєднання:", "Поєднує елементи двох послідовностей на основі рівності ключів."),
            ("Hash Match механіка:", "Join вичитує праву колекцію у хеш-таблицю, після чого потоково проходить по лівій колекції."),
            ("Швидкодія O(N + M):", "В рази швидше за вкладені цикли або вкладені Where, складність яких становить O(N * M)."),
            ("Елементи без пари:", "Записи без відповідників у протилежній таблиці відкидаються (аналог SQL INNER JOIN).")
        ],
        "⚡ Оптимізація пам'яті Join:",
        "У Join завжди передавайте як внутрішню (другу) колекцію ту, що містить менше записів, бо саме для неї будується хеш-таблиця в пам'яті."
    )

    add_code_panel(s21, 6.81, 1.55, 5.72, 4.25, [
        "// З'єднання товарів та категорій за текстовим ключем:",
        "var productCategories =",
        "    from c in categories",
        "    join p in products on c equals p.Category",
        "    select new { Category = c, p.ProductName };",
        "",
        "// Синтаксис методів (Fluent API):",
        "var joined = categories.Join(",
        "    products,                 // Inner sequence",
        "    c => c,                   // Outer key selector",
        "    p => p.Category,          // Inner key selector",
        "    (c, p) => new { c, p.ProductName } // Result selector",
        ");"
    ], "JoinOperators.cs (Linq102 - Linq103)")

    add_bottom_banner(s21, "💡 Оптимізація пам'яті Join:",
                      "У методі Join завжди передавайте як внутрішню (другу) колекцію ту, що містить меншу кількість елементів, оскільки саме для неї будується хеш-таблиця в оперативній пам'яті.")

    # ==========================================
    # SLIDE 22: GROUPJOIN & LEFT OUTER JOIN
    # ==========================================
    s22 = prs.slides.add_slide(blank_layout)
    add_bg(s22)
    add_header(s22, "06 · Реляційні з'єднання", "Ієрархічне З'єднання (GroupJoin) та Left Outer Join", 22, accent_rgb=ACCENT_AMBER)

    add_theory_card(s22, 0.8, 1.55, 5.72, 4.25,
        "Зв'язок 1:N та імітація LEFT JOIN", TEXT_MAIN,
        [
            ("GroupJoin (1:N):", "Для кожного лівого елемента повертає сам елемент і повну колекцію всіх його дочірніх відповідників."),
            ("Синтаксис into:", "Конструкція join ... into matchingGroup зберігає нащадків у вкладений IEnumerable."),
            ("Емуляція LEFT JOIN:", "Розгортання групи через from item in matchingGroup.DefaultIfEmpty()."),
            ("DefaultIfEmpty():", "Якщо група порожня, повертає [null], зберігаючи батьківський запис у вибірці.")
        ],
        "⚡ Золотий стандарт Left Join:",
        "У C# немає ключового слова 'left join'. Комбінація GroupJoin + DefaultIfEmpty є єдиним архітектурно правильним способом Left Join."
    )

    add_code_panel(s22, 6.81, 1.55, 5.72, 4.25, [
        "// Канонічний Left Outer Join постачальників та клієнтів:",
        "var leftOuterJoin =",
        "    from s in suppliers",
        "    join c in customers on s.Country equals c.Country into cs",
        "    from c in cs.DefaultIfEmpty() // Трюк для збереження s!",
        "    select new",
        "    {",
        "        s.Country,",
        "        s.SupplierName,",
        "        CustomerName = c?.CompanyName ?? \"(No customers)\"",
        "    };",
        "",
        "-> На виході: всі постачальники включно з тими, у кого немає клієнтів"
    ], "JoinOperators.cs (Linq104 - Linq105)")

    add_bottom_banner(s22, "💡 Золотий стандарт Left Join:",
                      "У C# немає ключового слова 'left join'. Комбінація GroupJoin + DefaultIfEmpty є єдиним архітектурно правильним способом вираження лівого зовнішнього з'єднання.")

    # ==========================================
    # SLIDE 23: BASIC AGGREGATES
    # ==========================================
    s23 = prs.slides.add_slide(blank_layout)
    add_bg(s23)
    add_header(s23, "06 · Реляційні з'єднання", "Базові Агрегатні Функції: Count, Sum, Min, Max, Average", 23, accent_rgb=ACCENT_AMBER)

    add_theory_card(s23, 0.8, 1.55, 5.72, 4.25,
        "Скалярна редукція послідовностей", TEXT_MAIN,
        [
            ("Термінальні оператори:", "Негайно вичитують потік і згортають його в одне скалярне значення."),
            ("Предикатний Count:", "source.Count(x => condition) працює швидше за ланцюжок source.Where(condition).Count()."),
            ("Селектори властивостей:", "Sum(p => p.Price), Average(...), Min(...) обчислюють метрики напряму без зайвого Select."),
            ("Небезпека аварії:", "Min/Max/Average на порожній колекції типів-значень викидають InvalidOperationException!")
        ],
        "⚡ Захист від порожніх вибірок:",
        "Приведення до Nullable типу: products.Average(p => (decimal?)p.UnitPrice) повертає null замість аварійної зупинки додатку."
    )

    add_code_panel(s23, 6.81, 1.55, 5.72, 4.25, [
        "// 1. Агрегація замовлень клієнта:",
        "var stats = customers.Select(c => new",
        "{",
        "    c.CustomerID,",
        "    TotalOrders = c.Orders.Count(),",
        "    TotalSpent  = c.Orders.Sum(o => o.Total),",
        "    MaxOrder    = c.Orders.Max(o => (decimal?)o.Total) ?? 0m",
        "});",
        "",
        "// 2. Вкладене групування з середньою ціною:",
        "var catAvg = products",
        "    .GroupBy(p => p.Category)",
        "    .Select(g => new { g.Key, AvgPrice = g.Average(p => p.UnitPrice) });"
    ], "AggregateOperators.cs (Linq73 - Linq90)")

    add_bottom_banner(s23, "⚠️ Захист від аварій:",
                      "Уникайте викликів Min / Max / Average на сирих послідовностях без перевірки Any() або приведення до Nullable-типів (int?, decimal?), щоб захистити додаток від збоїв на порожніх даних.", banner_type="warn")

    # ==========================================
    # SLIDE 24: AGGREGATE (FOLD / REDUCE)
    # ==========================================
    s24 = prs.slides.add_slide(blank_layout)
    add_bg(s24)
    add_header(s24, "06 · Реляційні з'єднання", "Універсальний Акумулятор: Aggregate (Fold / Reduce)", 24, accent_rgb=ACCENT_AMBER)

    add_theory_card(s24, 0.8, 1.55, 5.72, 4.25,
        "Фундаментальна операція згортки", TEXT_MAIN,
        [
            ("Оператор Aggregate:", "Найпотужніший акумулятор функціонального програмування (FoldL у Haskell, reduce в JS)."),
            ("Акумулятор стану Seed:", "Приймає початкове значення і функцію оновлення: (acc, next) => newAccumulator."),
            ("Універсальність:", "Оператори Sum, Count, Min, Max, string.Join є лише окремими випадками Aggregate."),
            ("Бізнес-задачі:", "Фінансові баланси з лімітами і комісіями, перемноження матриць, складні накопичувачі.")
        ],
        "⚡ Функціональна міць:",
        "Aggregate дозволяє елегантно виразити будь-яку накопичувальну бізнес-логіку без зовнішніх мутабельних змінних."
    )

    add_code_panel(s24, 6.81, 1.55, 5.72, 4.25, [
        "// 1. Зняття коштів з рахунку з комісією:",
        "double startBalance = 100.0;",
        "int[] attemptedWithdrawals = { 20, 10, 40, 50, 10, 20 };",
        "",
        "double finalBalance = attemptedWithdrawals.Aggregate(",
        "    startBalance, // Початковий Seed",
        "    (balance, nextWithdrawal) =>",
        "        (nextWithdrawal <= balance) ? (balance - nextWithdrawal) : balance",
        ");",
        "-> Output: Ending balance: 20",
        "",
        "// 2. Розрахунок факторіалу 5! = 120:",
        "int fact = Enumerable.Range(1, 5).Aggregate((acc, x) => acc * x);"
    ], "AggregateOperators.cs (Linq91 - Linq93)")

    add_bottom_banner(s24, "💡 Функціональна міць:",
                      "Оператор Aggregate дозволяє елегантно реалізовувати будь-яку бізнес-логіку накопичувального розрахунку без використання зовнішніх мутабельних змінних.")

    # ==========================================
    # SLIDE 25: SHORT-CIRCUITING ANY & ALL
    # ==========================================
    s25 = prs.slides.add_slide(blank_layout)
    add_bg(s25)
    add_header(s25, "07 · Advanced & Best Practices", "Квантори та Коротке Замикання: Any, All, Contains", 25, accent_rgb=ACCENT_TEAL)

    add_theory_card(s25, 0.8, 1.55, 5.72, 4.25,
        "Математична логіка ∃ та ∀", TEXT_MAIN,
        [
            ("Any() (Квантор ∃):", "Повертає true, якщо хоча б один елемент задовольняє умову. Миттєва зупинка (Short-circuit)."),
            ("All() (Квантор ∀):", "Повертає true, якщо всі елементи задовольняють умову. Миттєво зупиняється на першому false."),
            ("Contains(value):", "Швидка перевірка наявності конкретного значення за Equals/GetHashCode."),
            ("Критичний антипатерн:", "Перевірка if (list.Count() > 0) змушує сканувати весь мільйон елементів замість O(1) Any()!")
        ],
        "⚡ Швидкодія Short-circuiting:",
        "Any() завершує виконання після перевірки рівно одного елемента. Виграш становить мільйони непотрібних CPU операцій."
    )

    add_code_panel(s25, 6.81, 1.55, 5.72, 4.25, [
        "// ❌ ГРУБА ПОМИЛКА: Рахує 1... 2... 1 000 000 елементів!",
        "if (largeSequence.Count() > 0) { ... }",
        "",
        "// ✅ ОПТИМАЛЬНО: Зупиняється після 1-го ж елемента (O(1)):",
        "if (largeSequence.Any()) { ... }",
        "",
        "// Квантор All (перевірка наявності всіх товарів на складі):",
        "var fullyStockedCategories =",
        "    products",
        "        .GroupBy(p => p.Category)",
        "        .Where(g => g.All(p => p.UnitsInStock > 0));"
    ], "Quantifiers.cs (Linq67 - Linq72)")

    add_bottom_banner(s25, "⚠️ Правило №1 для будь-якого .NET розробника:",
                      "Ніколи не використовуйте .Count() > 0 для перевірки непустоти послідовності. Використовуйте виключно .Any() для гарантії O(1) складності.", banner_type="warn")

    # ==========================================
    # SLIDE 26: ELEMENT OPERATORS TABLE
    # ==========================================
    s26 = prs.slides.add_slide(blank_layout)
    add_bg(s26)
    add_header(s26, "07 · Advanced & Best Practices", "Вилучення Елементів: First, Single, ElementAt та *OrDefault", 26, accent_rgb=ACCENT_TEAL)

    rows = 4
    cols = 4
    t_shape = s26.shapes.add_table(rows, cols, Inches(0.8), Inches(1.55), Inches(11.733), Inches(4.25))
    table = t_shape.table
    table.columns[0].width = Inches(2.7)
    table.columns[1].width = Inches(2.9)
    table.columns[2].width = Inches(3.0)
    table.columns[3].width = Inches(3.133)

    headers = ["Оператор", "0 елементів (порожньо)", "Рівно 1 елемент", "2 і більше елементів"]
    data = [
        ["First()", "❌ InvalidOperationException", "✅ Повертає елемент", "✅ Повертає перший (зупинка)"],
        ["FirstOrDefault()", "✅ Повертає default(T) / null", "✅ Повертає елемент", "✅ Повертає перший"],
        ["Single()", "❌ InvalidOperationException", "✅ Повертає елемент", "❌ InvalidOperationException!"]
    ]

    for col_idx, text in enumerate(headers):
        cell = table.cell(0, col_idx)
        cell.fill.solid()
        cell.fill.fore_color.rgb = RGBColor(30, 41, 59)
        p = cell.text_frame.paragraphs[0]
        p.text = text
        p.font.name = "Calibri"
        p.font.size = Pt(13.5)
        p.font.bold = True
        p.font.color.rgb = RGBColor(255, 255, 255)

    for row_idx, row_vals in enumerate(data, 1):
        for col_idx, val in enumerate(row_vals):
            cell = table.cell(row_idx, col_idx)
            cell.fill.solid()
            cell.fill.fore_color.rgb = RGBColor(241, 245, 249) if row_idx % 2 == 1 else RGBColor(255, 255, 255)
            p = cell.text_frame.paragraphs[0]
            p.text = val
            p.font.name = "Calibri"
            p.font.size = Pt(12.5)
            p.font.color.rgb = TEXT_MAIN
            if col_idx == 0:
                p.font.bold = True
                p.font.color.rgb = ACCENT_INDIGO

    add_bottom_banner(s26, "⚠️ Чому Single() небезпечний без унікального індексу:",
                      "First() зупиняється на першому знайденому рядку. Single() зобов'язаний просканувати 100% таблиці до кінця, щоб переконатися у відсутності другого дубліката.", banner_type="warn")

    # ==========================================
    # SLIDE 27: CONVERSION & OFTYPE
    # ==========================================
    s27 = prs.slides.add_slide(blank_layout)
    add_bg(s27)
    add_header(s27, "07 · Advanced & Best Practices", "Матеріалізація та Фільтрація за Типом: ToDictionary та OfType", 27, accent_rgb=ACCENT_TEAL)

    add_dual_comparison_card(s27, 0.8, 1.55, 5.72, 4.25,
        "📖 Оператор ToDictionary", ACCENT_TEAL,
        [
            ("Швидкий пошук за O(1):", "Створює Dictionary<TKey, TElement> за заданим ключем."),
            ("Небезпека дублікатів:", "При дублікаті ключа викидає ArgumentException!"),
            ("Альтернатива:", "Якщо дублікати можливі — використовуйте ToLookup або GroupBy.")
        ],
        [
            "// Швидкий індекс товарів за ID:",
            "Dictionary<int, Product> map =",
            "    products.ToDictionary(p => p.ProductID);",
            "Product p5 = map[5]; // Пошук за O(1)",
            "-> Output: Chef Anton's Gumbo Mix"
        ]
    )

    add_dual_comparison_card(s27, 6.81, 1.55, 5.72, 4.25,
        "🛡️ Оператор OfType<T>()", ACCENT_EMERALD,
        [
            ("Безпечна фільтрація:", "Витягує лише елементи сумісного типу T з поліморфного списку."),
            ("Чому не Cast<T>:", "Cast викидає InvalidCastException; OfType безпечно пропускає невідповідні типи."),
            ("Поліморфізм:", "Ідеально для черг повідомлень та гетерогенних колекцій.")
        ],
        [
            "object[] mixed = { 1.5, \"два\", 3, \"чотири\", 5.0 };",
            "// Безпечне вилучення double чисел:",
            "var doubles = mixed.OfType<double>();",
            "-> Output: 1.5 5.0",
            "// Жодного InvalidCastException!"
        ]
    )

    add_bottom_banner(s27, "💡 Робота з наслідуванням:",
                      "Оператор OfType<T>() — канонічний спосіб обробки ієрархій класів, черг повідомлень та гетерогенних колекцій у Clean Architecture.")

    # ==========================================
    # SLIDE 28: CUSTOM OPERATORS (COMBINE)
    # ==========================================
    s28 = prs.slides.add_slide(blank_layout)
    add_bg(s28)
    add_header(s28, "07 · Advanced & Best Practices", "Створення Власних LINQ-Операторів: Патерн Combine / Zip", 28, accent_rgb=ACCENT_TEAL)

    add_theory_card(s28, 0.8, 1.55, 5.72, 4.25,
        "Як розширити мову LINQ", TEXT_MAIN,
        [
            ("Extension Method:", "Публічний статичний метод у статичному класі з ключовим словом this IEnumerable<T>."),
            ("Лінива генерація:", "Обов'язкове використання yield return всередині для підтримки потоковості."),
            ("Управління ресурсами:", "Ручне відкриття перелічувачів зобов'язане супроводжуватися using для виклику Dispose()."),
            ("Синхронна паралельна ітерація:", "Метод Combine (аналог Zip) зчитує по одному елементу з двох джерел одночасно.")
        ],
        "⚡ Патерн розширення мови LINQ через yield return:",
        "Створення власних операторів на базі yield return дозволяє легко інтегрувати специфічні бізнес-алгоритми в стандартні ланцюжки викликів."
    )

    add_code_panel(s28, 6.81, 1.55, 5.72, 4.25, [
        "public static class CustomSequenceExtensions",
        "{",
        "    public static IEnumerable<TResult> Combine<T1, T2, TResult>(",
        "        this IEnumerable<T1> first,",
        "        IEnumerable<T2> second,",
        "        Func<T1, T2, TResult> func)",
        "    {",
        "        using var e1 = first.GetEnumerator();",
        "        using var e2 = second.GetEnumerator();",
        "        while (e1.MoveNext() && e2.MoveNext())",
        "        {",
        "            yield return func(e1.Current, e2.Current);",
        "        }",
        "    }",
        "}"
    ], "CustomSequenceOperators.cs (DataSetLinq98.cs)")

    add_bottom_banner(s28, "💡 Безмежні можливості розширення:",
                      "Створення власних операторів на базі yield return дозволяє легко інтегрувати специфічні бізнес-алгоритми в стандартні ланцюжки викликів LINQ.")

    # ==========================================
    # SLIDE 29: TOP 7 ANTI-PATTERNS
    # ==========================================
    s29 = prs.slides.add_slide(blank_layout)
    add_bg(s29)
    add_header(s29, "07 · Advanced & Best Practices", "ТОП-7 Антипатернів LINQ та Золоті Правила Оптимізації", 29, accent_rgb=ACCENT_TEAL)

    c_l = s29.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(0.8), Inches(1.55), Inches(5.72), Inches(4.25))
    c_l.fill.solid()
    c_l.fill.fore_color.rgb = CARD_BG
    c_l.line.color.rgb = CARD_BORDER
    c_l.line.width = Pt(1)

    tb_l = s29.shapes.add_textbox(Inches(0.95), Inches(1.68), Inches(5.42), Inches(3.9))
    tf_l = tb_l.text_frame
    tf_l.word_wrap = True
    p = tf_l.paragraphs[0]
    p.text = "❌ Чого слід категорично уникати"
    p.font.name = "Calibri"
    p.font.size = Pt(17)
    p.font.bold = True
    p.font.color.rgb = ACCENT_CRIMSON

    bad_rules = [
        "1. list.Count() > 0 — сканує всю колекцію замість Any().",
        "2. list.OrderBy(a).OrderBy(b) — скидає перше сортування.",
        "3. Ранній .ToList() всередині циклів — зайве навантаження на Heap.",
        "4. Multiple Enumeration на важких IQueryable або файлах.",
        "5. Пошук у List.Contains() за O(N) замість HashSet O(1).",
        "6. Виклик Single() у високонавантажених сервісах.",
        "7. Модифікація колекції під час foreach ітерації."
    ]
    for r in bad_rules:
        pb = tf_l.add_paragraph()
        pb.space_before = Pt(6)
        pb.text = r
        pb.font.name = "Calibri"
        pb.font.size = Pt(12.5)
        pb.font.color.rgb = TEXT_MAIN

    c_r = s29.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(6.81), Inches(1.55), Inches(5.72), Inches(4.25))
    c_r.fill.solid()
    c_r.fill.fore_color.rgb = CARD_BG
    c_r.line.color.rgb = CARD_BORDER
    c_r.line.width = Pt(1)

    tb_r = s29.shapes.add_textbox(Inches(6.96), Inches(1.68), Inches(5.42), Inches(3.9))
    tf_r = tb_r.text_frame
    tf_r.word_wrap = True
    p = tf_r.paragraphs[0]
    p.text = "✅ Золотий стандарт Enterprise-розробки"
    p.font.name = "Calibri"
    p.font.size = Pt(17)
    p.font.bold = True
    p.font.color.rgb = ACCENT_EMERALD

    good_rules = [
        "1. Завжди Any() для перевірки наявності даних за O(1).",
        "2. OrderBy() + ThenBy() для багатовимірних ключів.",
        "3. Матеріалізація (ToList) строго в самому кінці конвеєра.",
        "4. Збереження результату у змінну при 2+ зверненнях.",
        "5. Попередня побудова HashSet або Dictionary для пошуку.",
        "6. FirstOrDefault() за умовчанням або індекс для Single().",
        "7. Фільтрація Where та пагінація Take ДО сортування OrderBy."
    ]
    for r in good_rules:
        pb = tf_r.add_paragraph()
        pb.space_before = Pt(6)
        pb.text = r
        pb.font.name = "Calibri"
        pb.font.size = Pt(12.5)
        pb.font.color.rgb = TEXT_MAIN

    add_bottom_banner(s29, "🏆 Архітектурне резюме:",
                      "LINQ дарує бездоганну читабельність та елегантність коду, але розуміння складності O(N), моделей виконання та виділення пам'яті — ознака справжнього Senior інженера.")

    # ==========================================
    # SLIDE 30: FINAL SUMMARY
    # ==========================================
    s30 = prs.slides.add_slide(blank_layout)
    bg30 = s30.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(0), Inches(0), Inches(13.333), Inches(7.500))
    bg30.fill.solid()
    bg30.fill.fore_color.rgb = RGBColor(15, 23, 42)
    bg30.line.fill.background()

    tb30 = s30.shapes.add_textbox(Inches(0.8), Inches(1.2), Inches(11.7), Inches(2.2))
    tf30 = tb30.text_frame
    tf30.word_wrap = True
    p30_1 = tf30.paragraphs[0]
    p30_1.text = "Дякую за увагу!\nПрактикуйтеся на реальних прикладах"
    p30_1.font.name = "Calibri"
    p30_1.font.size = Pt(36)
    p30_1.font.bold = True
    p30_1.font.color.rgb = RGBColor(255, 255, 255)

    p30_sub = tf30.add_paragraph()
    p30_sub.text = "Повний навчальний репозиторій з усіма 101 прикладами доступний у вашому рішенні:"
    p30_sub.font.name = "Calibri"
    p30_sub.font.size = Pt(18)
    p30_sub.font.color.rgb = RGBColor(148, 163, 184)

    repo_boxes = [
        ("🚀 КОНСОЛЬНИЙ РАННЕР", "dotnet run --project \"101 LINQ Samples\"", "Інтерактивне меню запуску будь-якого зі 101 прикладів з виводом у термінал"),
        ("📘 ПІДРУЧНИК У DOCS/", "6 детальних посібників у Markdown", "Повні конспекти лекцій, діаграми Mermaid та прямі посилання на файли коду"),
        ("⚡ .NET 10 LTS READY", "0 Errors · 0 Warnings", "Повна підтримка C# 14, швидкісний SDK-style та чиста модульна архітектура")
    ]
    for i, (head, sub, desc) in enumerate(repo_boxes):
        cx = 0.8 + i * 4.0
        c = s30.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(cx), Inches(4.0), Inches(3.73), Inches(2.5))
        c.fill.solid()
        c.fill.fore_color.rgb = RGBColor(30, 41, 59)
        c.line.color.rgb = RGBColor(51, 65, 85)
        c.line.width = Pt(1)
        tf_c = c.text_frame
        tf_c.word_wrap = True
        p_c1 = tf_c.paragraphs[0]
        p_c1.text = head
        p_c1.font.name = "Calibri"
        p_c1.font.size = Pt(15)
        p_c1.font.bold = True
        p_c1.font.color.rgb = ACCENT_SKY

        p_c2 = tf_c.add_paragraph()
        p_c2.text = sub
        p_c2.font.name = "Calibri"
        p_c2.font.size = Pt(13)
        p_c2.font.bold = True
        p_c2.font.color.rgb = RGBColor(255, 255, 255)

        p_c3 = tf_c.add_paragraph()
        p_c3.text = desc
        p_c3.font.name = "Calibri"
        p_c3.font.size = Pt(11)
        p_c3.font.color.rgb = RGBColor(148, 163, 184)

    script_dir = os.path.dirname(os.path.abspath(__file__))
    out_file = os.path.join(script_dir, "101-LINQ-Architecture-and-Operators.pptx")
    prs.save(out_file)
    print(f"Presentation saved successfully to {out_file} (Total slides: {len(prs.slides)})")

    # Mirror to '101 LINQ Samples/pptx'
    mirror_dir = os.path.abspath(os.path.join(script_dir, "..", "101 LINQ Samples", "pptx"))
    if os.path.exists(mirror_dir):
        import shutil
        mirror_file = os.path.join(mirror_dir, "101-LINQ-Architecture-and-Operators.pptx")
        shutil.copyfile(out_file, mirror_file)
        print(f"Mirrored successfully to {mirror_file}")

if __name__ == "__main__":
    build_presentation()
