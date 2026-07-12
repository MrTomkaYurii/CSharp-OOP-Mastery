from pptx.util import Inches, Pt
from pptx.dml.color import RGBColor
from tools.builder_core import (
    add_bg, add_header, add_bottom_banner, add_code_panel,
    add_theory_card, add_dual_comparison_card, add_table_card,
    ACCENT_INDIGO, ACCENT_SKY, ACCENT_EMERALD, ACCENT_VIOLET,
    ACCENT_AMBER, ACCENT_TEAL, ACCENT_CRIMSON
)

def build_part2(prs, blank_layout):
    print("Building Part 2 (Slides 37 - 72)...")

    # ==========================================
    # SLIDE 37: JOINING OPERATORS ТАБЛИЦЯ
    # ==========================================
    s37 = prs.slides.add_slide(blank_layout)
    add_bg(s37)
    add_header(s37, "08 · З'єднання (Joining)", "Оператори реляційного з'єднання: Join та GroupJoin", 37)
    headers_37 = ["Метод LINQ", "Опис / Призначення", "Що робить", "Аналог у SQL", "Приклад", "Результат"]
    rows_37 = [
        ["Join", "Об'єднує дві колекції за спільним ключем і повертає послідовність пар елементів.", "Виконує внутрішнє об'єднання (inner join): бере лише ті елементи, для яких є збіг в обох колекціях.", "INNER JOIN", "students.Join(standards, s => s.StandardID, st => st.StandardID, (s, st) => new { s.Name, st.Name })", "Список студентів разом із назвами їхніх класів"],
        ["GroupJoin", "Об'єднує дві колекції за ключем, але повертає групи пов'язаних елементів (1:N).", "Створює колекцію груп, де кожен батьківський елемент містить підколекцію дочірніх.", "LEFT OUTER JOIN", "standards.GroupJoin(students, st => st.StandardID, s => s.StandardID, (st, stds) => new { st.Name, Students = stds })", "Кожен клас зі списком усіх своїх студентів"]
    ]
    add_table_card(s37, 0.8, 1.55, 11.733, 1.85, headers_37, rows_37, [1.5, 2.5, 2.4, 1.4, 2.2, 1.733], ACCENT_INDIGO)
    add_code_panel(s37, 0.8, 3.55, 11.733, 2.25, [
        "// 1. Внутрішнє з'єднання Join (Inner Join): плаский список пар студент-клас",
        'var inner = studentList.Join(standardList, s => s.StandardID, st => st.StandardID,',
        '    (s, st) => new { s.StudentName, st.StandardName });',
        'foreach (var item in inner) Console.WriteLine($"{item.StudentName} -> {item.StandardName}");',
        '',
        "// 2. Групове з'єднання GroupJoin (Ієрархічне 1:N): кожен клас містить список студентів",
        'var groups = standardList.GroupJoin(studentList, st => st.StandardID, s => s.StandardID,',
        '    (st, stds) => new { st.StandardName, Students = stds });',
        'foreach (var g in groups) Console.WriteLine($"{g.StandardName}: {g.Students.Count()} учнів");'
    ], 'JoinVsGroupJoin.cs')
    add_bottom_banner(s37, "💡 Різниця структур:",
                      "Join створює плаский денормалізований список рядків, тоді як GroupJoin створює нормалізоване ієрархічне дерево 1:N.")
    # ==========================================
    # SLIDE 38: ПРИКЛАД JOIN НА ПРОСТИХ СПИСКАХ
    # ==========================================
    s38 = prs.slides.add_slide(blank_layout)
    add_bg(s38)
    add_header(s38, "08 · З'єднання (Joining)", "Практичний приклад: Внутрішнє з'єднання двох списків рядків", 38)
    add_theory_card(s38, 0.8, 1.55, 5.72, 4.25,
        'Зіставлення рядкових колекцій за спільними значеннями', ACCENT_INDIGO,
        [
            ('Колекція 1 (Outer):', "strList1 містить елементи: 'One', 'Two', 'Three', 'Four'."),
            ('Колекція 2 (Inner):', "strList2 містить елементи: 'One', 'Two', 'Five', 'Six'."),
            ("З'єднання Join:", 'Зіставляє елементи обох списків за рівністю рядків str1 == str2.'),
            ('Хешування ключів:', "Для strList2 будується швидка хеш-таблиця у пам'яті для миттєвого пошуку O(1).")
        ],
        '⚡ Результат Inner Join:',
        "Повертає лише спільні елементи: 'One' та 'Two'. Усі незбіжні елементи ('Three', 'Four', 'Five', 'Six') відкидаються."
    )
    add_code_panel(s38, 6.81, 1.55, 5.72, 4.25, [
        '// Дві вхідні колекції:',
        'IList<string> strList1 = new List<string>() {',
        '    "One", "Two", "Three", "Four"',
        '};',
        'IList<string> strList2 = new List<string>() {',
        '    "One", "Two", "Five", "Six"',
        '};',
        '',
        "// Внутрішнє об'єднання за рівністю рядків:",
        'var innerJoin = strList1.Join(',
        '    strList2,               // внутрішня колекція',
        '    str1 => str1,           // ключ зовнішньої',
        '    str2 => str2,           // ключ внутрішньої',
        '    (str1, str2) => str1);  // формування результату',
        '',
        'foreach (var str in innerJoin)',
        '    Console.WriteLine($"Спільний елемент: {str}");',
        '',
        '// -> Output:',
        '// -> Спільний елемент: One',
        '// -> Спільний елемент: Two'
    ], 'Program.cs')
    add_bottom_banner(s38, "💡 Складність Hash Join:",
                      "LINQ будує внутрішню хеш-таблицю для strList2, тому пошук спільних елементів виконується за лінійний час O(N + M).")
    # ==========================================
    # SLIDE 39: СТУДЕНТСЬКІ СУТНОСТІ ТА JOIN
    # ==========================================
    s39 = prs.slides.add_slide(blank_layout)
    add_bg(s39)
    add_header(s39, "08 · З'єднання (Joining)", "Реляційні класи Student і Standard та з'єднання сутностей", 39)
    add_theory_card(s39, 0.8, 1.55, 5.72, 4.25,
        'Модель реляційних даних у C#', ACCENT_INDIGO,
        [
            ('Клас Student:', 'Містить поля StudentID, StudentName та зовнішній ключ StandardID (посилання на клас).'),
            ('Клас Standard:', 'Містить первинний ключ StandardID та текстову назву класу StandardName.'),
            ("Реляційний зв'язок:", "Кожен студент прив'язаний до певного стандарту через зовнішній ключ StandardID."),
            ("Проекція зв'язку:", "Join дозволяє з'єднати сутності та створити анонімний об'єкт із полями з обох таблиць.")
        ],
        "⚡ З'єднання сутностей:",
        "З'єднуємо таблицю студентів із таблицею стандартів за рівністю student.StandardID == standard.StandardID."
    )
    add_code_panel(s39, 6.81, 1.55, 5.72, 4.25, [
        "// З'єднання сутностей студентів та стандартів:",
        'var innerJoin = studentList.Join(',
        '    standardList,',
        '    student => student.StandardID,',
        '    standard => standard.StandardID,',
        '    (student, standard) => new {',
        '        StudentName = student.StudentName,',
        '        StandardName = standard.StandardName',
        '    });',
        '',
        "// Виведення результатів з'єднання:",
        'foreach (var obj in innerJoin)',
        '{',
        '    Console.WriteLine($"{obj.StudentName} навчається у {obj.StandardName}");',
        '}',
        '',
        '// -> Output:',
        '// -> John навчається у Standard 1',
        '// -> Moin навчається у Standard 1',
        '// -> Bill навчається у Standard 2'
    ], 'Program.cs')
    add_bottom_banner(s39, "💡 Результат проекції:",
                      "Отримуємо кортежі: John -> Standard 1, Moin -> Standard 1, Bill -> Standard 2, Ram -> Standard 2.")
    # ==========================================
    # SLIDE 40: АНАТОМІЯ МЕТОДУ JOIN
    # ==========================================
    s40 = prs.slides.add_slide(blank_layout)
    add_bg(s40)
    add_header(s40, "08 · З'єднання (Joining)", "Анатомія параметрів оператора Join у Method Syntax", 40)
    add_theory_card(s40, 0.8, 1.55, 5.72, 4.25,
        "П'ять ключових компонентів методу Join", ACCENT_SKY,
        [
            ('1. Outer Sequence (Зовнішня):', 'Колекція, на якій викликається метод (studentList).'),
            ('2. Inner Sequence (Внутрішня):', 'Друга колекція, що передається першим аргументом (standardList).'),
            ('3. Key Selectors:', 'Дві лямбди вибору ключів: student => student.StandardID та standard => standard.StandardID.'),
            ('4. Result Selector:', "Функція формування об'єкта результату (student, standard) => new { ... }.")
        ],
        '⚡ Правило оптимізації Hash Join:',
        'LINQ будує хеш-таблицю для Inner Sequence. Рекомендується передавати меншу за обсягом таблицю як Inner Sequence!'
    )
    add_code_panel(s40, 6.81, 1.55, 5.72, 4.25, [
        '// Анатомія параметрів методу Join:',
        'var joinResult = studentList.Join(',
        '    standardList,                   // 1. Inner Sequence',
        '    student => student.StandardID,  // 2. Outer Key Selector',
        '    standard => standard.StandardID,// 3. Inner Key Selector',
        '    (student, standard) => new {    // 4. Result Selector',
        '        StudentFullName = student.StudentName,',
        '        StandardFullName = standard.StandardName',
        '    });',
        '',
        'foreach (var item in joinResult)',
        '    Console.WriteLine($"{item.StudentFullName} -> {item.StandardFullName}");',
        '',
        '// -> Output:',
        '// -> John -> Standard 1',
        '// -> Moin -> Standard 1',
        '// -> Bill -> Standard 2',
        '// -> Ram -> Standard 2'
    ], 'Program.cs')
    add_bottom_banner(s40, "💡 Безпека типів:",
                      "Типи ключів OuterKey та InnerKey повинні точно збігатися (наприклад, обидва мають бути int).")
    # ==========================================
    # SLIDE 41: JOIN У QUERY SYNTAX
    # ==========================================
    s41 = prs.slides.add_slide(blank_layout)
    add_bg(s41)
    add_header(s41, "08 · З'єднання (Joining)", "Оператор Join у Query Syntax (join ... on ... equals ...)", 41)
    add_theory_card(s41, 0.8, 1.55, 5.72, 4.25,
        "Синтаксис реляційного з'єднання запитів", ACCENT_INDIGO,
        [
            ('join <var> in <col>:', 'Оголошує внутрішню колекцію та змінну для її елементів.'),
            ('on <key1> equals <key2>:', 'Вказує умову рівності ключів. Увага: використовується слово equals, а не ==!'),
            ('select new { ... }:', "Формує фінальний результат проекції об'єднаних полів."),
            ('Трансляція Roslyn:', 'Компілятор перетворює цю конструкцію на виклик Enumerable.Join у Method Syntax.')
        ],
        '⚡ Чому equals, а не ==:',
        'Ключове слово equals підкреслює семантику еквівалентності та використання EqualityComparer<T>.Default.'
    )
    add_code_panel(s41, 6.81, 1.55, 5.72, 4.25, [
        "// Query Syntax реляційного з'єднання:",
        'var innerJoin = from s in studentList',
        '                join st in standardList',
        '                on s.StandardID equals st.StandardID',
        '                select new {',
        '                    StudentName = s.StudentName,',
        '                    StandardName = st.StandardName',
        '                };',
        '',
        'foreach (var element in innerJoin)',
        '{',
        '    Console.WriteLine($"{element.StudentName} [{element.StandardName}]");',
        '}',
        '',
        '// -> Output:',
        '// -> John [Standard 1]',
        '// -> Moin [Standard 1]',
        '// -> Bill [Standard 2]',
        '// -> Ram [Standard 2]'
    ], 'Program.cs')
    add_bottom_banner(s41, "💡 Складені ключі:",
                      "Для з'єднання за кількома колонками використовуйте анонімні типи: on new { s.A, s.B } equals new { st.A, st.B }.")
    # ==========================================
    # SLIDE 42: ПРАВИЛА ОПЕРАТОРА JOIN
    # ==========================================
    s42 = prs.slides.add_slide(blank_layout)
    add_bg(s42)
    add_header(s42, "08 · З'єднання (Joining)", "Оператор GroupJoin: Ієрархічне з'єднання 1:N у Method Syntax", 42)
    add_theory_card(s42, 0.8, 1.55, 5.72, 4.25,
        "Групування зв'язаних даних без денормалізації", ACCENT_SKY,
        [
            ('Суть GroupJoin:', "З'єднує дві колекції, групуючи всі дочірні елементи під кожним батьківським об'єктом."),
            ('Колекція у результаті:', 'Селектор результату приймає батька та IEnumerable<Child> (список студентів для цього класу).'),
            ('Порожні групи:', 'Якщо для стандарту немає жодного студента, повертається порожня група (без відкидання батька!).'),
            ('Аналог Left Join:', 'GroupJoin є природною основою для реалізації SQL Left Outer Join у мові C#.')
        ],
        '⚡ Головна перевага GroupJoin:',
        "Батьківські елементи ніколи не дублюються! Замість 10 рядків з однаковим класом ви отримуєте 1 об'єкт класу зі списком учнів."
    )
    add_code_panel(s42, 6.81, 1.55, 5.72, 4.25, [
        "// Ієрархічне з'єднання 1:N через GroupJoin:",
        'var groupJoin = standardList.GroupJoin(',
        '    studentList,                    // дочірня колекція',
        '    std => std.StandardID,          // ключ батька',
        '    s => s.StandardID,              // ключ дитини',
        '    (std, students) => new {        // результат',
        '        Standard = std.StandardName,',
        '        Students = students',
        '    });',
        '',
        'foreach (var item in groupJoin)',
        '{',
        '    Console.WriteLine($"Клас: {item.Standard}");',
        '    foreach (var stud in item.Students)',
        '        Console.WriteLine($"   * {stud.StudentName}");',
        '}',
        '',
        '// -> Output:',
        '// -> Клас: Standard 1 -> John, Moin',
        '// -> Клас: Standard 2 -> Bill, Ram, Ron'
    ], 'Program.cs')
    add_bottom_banner(s42, "💡 Об'єктна модель:",
                      "GroupJoin ідеально лягає на реляційні зв'язки в базах даних: Один факультет -> Багато кафедр -> Багато студентів.")
    # ==========================================
    # SLIDE 43: GROUPJOIN ВСТУП
    # ==========================================
    s43 = prs.slides.add_slide(blank_layout)
    add_bg(s43)
    add_header(s43, "08 · З'єднання (Joining)", "Практичний приклад: Повна ітерація GroupJoin у C#", 43)
    add_theory_card(s43, 0.8, 1.55, 5.72, 4.25,
        'Аналіз ієрархічного виведення даних', ACCENT_INDIGO,
        [
            ('Батьківський перебір:', "Зовнішній цикл перебирає об'єкти стандартів (Standard 1, Standard 2, Standard 3)."),
            ('Дочірній перебір:', 'Внутрішній цикл перебирає колекцію студентів item.Students для кожного стандарту.'),
            ('Standard 3 без студентів:', 'Standard 3 залишається у результаті, але його колекція студентів має Count == 0.'),
            ('Без винятків NullReference:', 'item.Students завжди є валідним пустим перелічувачем Enumerable.Empty, а не null.')
        ],
        '⚡ Порівняння з Inner Join:',
        'В Inner Join Standard 3 зник би повністю! У GroupJoin Standard 3 зберігається з порожнім списком.'
    )
    add_code_panel(s43, 6.81, 1.55, 5.72, 4.25, [
        '// Виконання та перебір результатів GroupJoin:',
        'var groupJoin = standardList.GroupJoin(',
        '    studentList,',
        '    std => std.StandardID,',
        '    s => s.StandardID,',
        '    (std, students) => new {',
        '        Standard = std.StandardName,',
        '        Students = students',
        '    });',
        '',
        'foreach (var item in groupJoin)',
        '{',
        '    Console.WriteLine($"Клас: {item.Standard}");',
        '    foreach (var stud in item.Students)',
        '        Console.WriteLine($"  - {stud.StudentName}");',
        '}',
        '',
        '// -> Output:',
        '// -> Клас: Standard 1 -> John, Moin',
        '// -> Клас: Standard 2 -> Bill, Ram, Ron',
        '// -> Клас: Standard 3 (порожньо)'
    ], 'Program.cs')
    add_bottom_banner(s43, "💡 Пам'ять процесу:",
                      "GroupJoin не створює проміжних списків List<T> — ітерація по студентах відбувається ліниво на вимогу.")
    # ==========================================
    # SLIDE 44: GROUPJOIN У METHOD SYNTAX
    # ==========================================
    s44 = prs.slides.add_slide(blank_layout)
    add_bg(s44)
    add_header(s44, "08 · З'єднання (Joining)", "GroupJoin у Query Syntax: Конструкція join ... into ...", 44)
    add_theory_card(s44, 0.8, 1.55, 5.72, 4.25,
        'Ключове слово into для створення груп', ACCENT_SKY,
        [
            ('Синтаксис join into:', 'Додавання into <groupName> після умови equals перетворює звичайний join на GroupJoin.'),
            ('Змінна studentGroup:', 'Стає локальною колекцією типу IEnumerable<Student>, що містить усіх учнів поточного класу.'),
            ('Фінальна проекція:', 'У блоці select можна призначити всю групу у властивість анонімного типу або DTO.'),
            ('Чіткість та лаконічність:', 'Query Syntax для GroupJoin часто читається легше, ніж довгий 5-параметричний методний ланцюжок.')
        ],
        '⚡ Ключова відмінність:',
        'Без into: кожен збіг створює окремий плаский рядок. З into: усі збіги упаковуються в єдину локальну групу.'
    )
    add_code_panel(s44, 6.81, 1.55, 5.72, 4.25, [
        '// GroupJoin у декларативному Query Syntax:',
        'var groupJoin = from std in standardList',
        '                join s in studentList',
        '                on std.StandardID equals s.StandardID',
        '                into studentGroup',
        '                select new {',
        '                    Students = studentGroup,',
        '                    StandardName = std.StandardName',
        '                };',
        '',
        'foreach (var g in groupJoin)',
        '{',
        '    Console.WriteLine($"{g.StandardName}: {g.Students.Count()} учнів");',
        '}',
        '',
        '// -> Output:',
        '// -> Standard 1: 2 учнів',
        '// -> Standard 2: 3 учнів',
        '// -> Standard 3: 0 учнів'
    ], 'Program.cs')
    add_bottom_banner(s44, "💡 Ключове правило:",
                      "Звичайний join створює плаский декартовий результат; додавання слова into перетворює його на GroupJoin.")
    # ==========================================
    # SLIDE 45: GROUPJOIN У QUERY SYNTAX
    # ==========================================
    s45 = prs.slides.add_slide(blank_layout)
    add_bg(s45)
    add_header(s45, "08 · З'єднання (Joining)", "Детальний розбір запиту GroupJoin у Query Syntax", 45)
    add_theory_card(s45, 0.8, 1.55, 5.72, 4.25,
        'Анатомія конструкції join into', ACCENT_INDIGO,
        [
            ('from std in standardList:', 'Визначає базове джерело (зовнішня послідовність стандартів).'),
            ('join s in studentList:', "Підключає пов'язану колекцію студентів для пошуку збігів."),
            ('on std.StandardID equals s.StandardID:', "Критерій зв'язку за ідентифікатором класу."),
            ('into studentGroup:', "Матеріалізує вибірку пов'язаних студентів у типізовану підколекцію.")
        ],
        '⚡ Роль into:',
        'studentGroup є локальною змінною діапазону типу IEnumerable<Student>, доступною в наступних пропозиціях запиту.'
    )
    add_code_panel(s45, 6.81, 1.55, 5.72, 4.25, [
        '// Повний вираз запиту GroupJoin:',
        'var studentsGroup = from std in standardList',
        '                    join s in studentList',
        '                    on std.StandardID equals s.StandardID',
        '                    into studentGroup',
        '                    select new {',
        '                        Students = studentGroup,',
        '                        StandardName = std.StandardName',
        '                    };',
        '',
        '// Дворівневий перебір сформованої ієрархії:',
        'foreach (var group in studentsGroup)',
        '{',
        '    Console.WriteLine(group.StandardName);',
        '    foreach (var s in group.Students)',
        '        Console.WriteLine($"   -> {s.StudentName}");',
        '}',
        '',
        '// -> Output: Standard 1 (John, Moin), Standard 2 (Bill, Ram, Ron)'
    ], 'Program.cs')
    add_bottom_banner(s45, "💡 Ключове правило:",
                      "Звичайний join створює плаский декартовий результат; додавання слова into перетворює його на GroupJoin.")
    # ==========================================
    # SLIDE 46: ПРАКТИКА GROUPJOIN QUERY SYNTAX
    # ==========================================
    s46 = prs.slides.add_slide(blank_layout)
    add_bg(s46)
    add_header(s46, "08 · З'єднання (Joining)", "Практичний приклад: Повна ітерація GroupJoin у C#", 46)
    add_theory_card(s46, 0.8, 1.55, 5.72, 4.25,
        'Аналіз консольного виведення ієрархії', ACCENT_SKY,
        [
            ('Standard 1:', 'Містить двох студентів: John та Moin.'),
            ('Standard 2:', 'Містить трьох студентів: Bill, Ram та Ron.'),
            ('Standard 3:', 'Порожня група, оскільки жоден студент не має StandardID = 3.'),
            ('Трансформація в Left Join:', 'Виклик DefaultIfEmpty() на групі перетворює її на повноцінний SQL LEFT OUTER JOIN.')
        ],
        '⚡ Як отримати Left Outer Join:',
        'Якщо після join into викликати from s in studentGroup.DefaultIfEmpty(), ми отримаємо класичний плаский SQL LEFT JOIN!'
    )
    add_code_panel(s46, 6.81, 1.55, 5.72, 4.25, [
        '// Реалізація плаского Left Outer Join через DefaultIfEmpty():',
        'var leftOuterJoin = from std in standardList',
        '                    join s in studentList',
        '                    on std.StandardID equals s.StandardID',
        '                    into studentGroup',
        '                    from s in studentGroup.DefaultIfEmpty()',
        '                    select new {',
        '                        Standard = std.StandardName,',
        '                        Student = s != null ? s.StudentName : "[НЕМАЄ]"',
        '                    };',
        '',
        'foreach (var row in leftOuterJoin)',
        '    Console.WriteLine($"{row.Standard}: {row.Student}");',
        '',
        '// -> Output:',
        '// -> Standard 1: John',
        '// -> Standard 1: Moin',
        '// -> Standard 2: Bill',
        '// -> Standard 3: [НЕМАЄ]'
    ], 'Program.cs')
    add_bottom_banner(s46, "💡 Порада з оптимізації:",
                      "GroupJoin набагато ефективніший за ручне знаходження дітей через standardList.Select(std => studentList.Where(...)).")
    # ==========================================
    # SLIDE 47: PROJECTION OPERATORS ВСТУП
    # ==========================================
    s47 = prs.slides.add_slide(blank_layout)
    add_bg(s47)
    add_header(s47, "09 · Проекція (Projection)", "Оператори проекції: Select та SelectMany", 47)
    add_dual_comparison_card(s47, 0.8, 1.55, 5.72, 4.25,
        "Оператор Select (Проекція 1:1)", ACCENT_INDIGO,
        [
            ("Призначення:", "Трансформує кожен елемент послідовності в нову форму."),
            ("Розмірність:", "Кількість елементів зберігається строго однаковою (N -> N)."),
            ("Цільові типи:", "Окремі властивості, анонімні типи, класи DTO, Records.")
        ],
        [
            "// Одновимірна проекція в анонімний тип:",
            "var studentNames = studentList.Select(s => new {",
            "    Name = s.StudentName,",
            "    Age = s.Age",
            "});",
            "// N вхідних елементів -> N вихідних об'єктів"
        ]
    )
    add_dual_comparison_card(s47, 6.81, 1.55, 5.72, 4.25,
        "Оператор SelectMany (Сплющення 1:N)", ACCENT_EMERALD,
        [
            ("Призначення:", "Розгортає вкладені колекції в єдиний плаский потік (flatMap)."),
            ("Розмірність:", "Кількість вихідних елементів може бути більшою або меншою (N -> M)."),
            ("Сценарії:", "Студент -> Курси, Клієнт -> Замовлення, Декартів добуток.")
        ],
        [
            "// Сплющення списку списків у плоский потік:",
            "var allCourses = students.SelectMany(s => s.Courses);",
            "",
            "// Було: List<List<Course>>",
            "// Стало: плоский IEnumerable<Course>"
        ]
    )
    add_bottom_banner(s47, "💡 Функціональна суть:",
                      "Select — це математичний Functor Map, а SelectMany — це монадний Bind/FlatMap у теорії категорій.")

    # ==========================================
    # SLIDE 48: ПРИКЛАД SELECT У QUERY SYNTAX
    # ==========================================
    s48 = prs.slides.add_slide(blank_layout)
    add_bg(s48)
    add_header(s48, "09 · Проекція (Projection)", "Практичний приклад Select у Query Syntax C#", 48)
    add_theory_card(s48, 0.8, 1.55, 5.72, 4.25,
        'Проекція в анонімні типи та властивості', ACCENT_SKY,
        [
            ('Вибірка підмножини полів:', "Не обов'язково передавати важкі сутності цілком — можна вибрати лише Name та Age."),
            ('Анонімні типи new { ... }:', "Створюють компактні легковажні об'єкти без ручного опису окремого класу DTO."),
            ('Перейменування полів:', 'У select можна перейменовувати властивості: select new { FullName = s.StudentName }.'),
            ('Проекція обчислень:', 'Можна обчислювати нові значення прямо у виразі: select new { s.StudentName, IsAdult = s.Age >= 18 }.')
        ],
        "⚡ Економія пам'яті:",
        'Проекція лише потрібних колонок у базах даних генерує точний SQL SELECT Name, Age FROM... замість SELECT *.'
    )
    add_code_panel(s48, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Проекція в анонімний тип:',
        'var selectResult = from s in studentList',
        '                   select new {',
        '                       Name = s.StudentName,',
        '                       Age = s.Age,',
        '                       IsAdult = s.Age >= 18',
        '                   };',
        '',
        "// 2. Ітерація по проектованих об'єктах:",
        'foreach (var item in selectResult)',
        '{',
        '    Console.WriteLine($"{item.Name} ({item.Age} р.) - Повнолітній: {item.IsAdult}");',
        '}',
        '',
        '// -> Output:',
        '// -> John (13 р.) - Повнолітній: False',
        '// -> Moin (21 р.) - Повнолітній: True',
        '// -> Bill (18 р.) - Повнолітній: True'
    ], 'Program.cs')
    add_bottom_banner(s48, "💡 Як працює анонімний тип:",
                      "Roslyn генерує незмінний узагальнений клас із перевизначеними Equals() та GetHashCode() за значенням властивостей.")
    # ==========================================
    # SLIDE 49: QUANTIFIER OPERATORS ТАБЛИЦЯ
    # ==========================================
    s49 = prs.slides.add_slide(blank_layout)
    add_bg(s49)
    add_header(s49, "10 · Квантори (Quantifiers)", "Оператори кванторів: All, Any та Contains", 49)
    headers_49 = ["Метод LINQ", "Опис", "Що робить", "Повертає", "Приклад", "Результат"]
    rows_49 = [
        ["All", "Перевіряє, чи всі елементи колекції задовольняють умову.", "Повертає true, якщо кожен елемент задовольняє предикат.", "bool", "[2, 4, 6].All(x => x % 2 == 0)", "true"],
        ["Any", "Перевіряє, чи хоча б один елемент відповідає умові.", "Повертає true, як тільки знайдено перший відповідний елемент.", "bool", "[1, 2, 3].Any(x => x % 2 == 0)", "true"],
        ["Contains", "Перевіряє, чи містить колекція конкретне значення.", "Повертає true, якщо заданий елемент знайдено у колекції.", "bool", "[1, 2, 3].Contains(2)", "true"]
    ]
    add_table_card(s49, 0.8, 1.55, 11.733, 2.10, headers_49, rows_49, [1.5, 2.7, 2.8, 1.2, 2.3, 1.233], ACCENT_INDIGO)
    add_code_panel(s49, 0.8, 3.8, 11.733, 2.0, [
        'int[] numbers = { 2, 4, 6, 8, 10 };',
        'bool allEven = numbers.All(n => n % 2 == 0);       // True (всі парні)',
        'bool hasTen  = numbers.Any(n => n == 10);         // True (зупинка на першому 10)',
        'bool hasFive = numbers.Contains(5);               // False (числа 5 немає)',
        'Console.WriteLine($"AllEven: {allEven}, HasTen: {hasTen}, ContainsFive: {hasFive}");',
        '// Output: AllEven: True, HasTen: True, ContainsFive: False'
    ], 'QuantifiersDemo.cs')
    add_bottom_banner(s49, "💡 Short-circuiting механіка:",
                      "Any зупиняється на першому true, All зупиняється на першому false — вони ніколи не перевіряють зайві елементи даремно.")
    # ==========================================
    # SLIDE 50: ОПЕРАТОР ALL
    # ==========================================
    s50 = prs.slides.add_slide(blank_layout)
    add_bg(s50)
    add_header(s50, "10 · Квантори (Quantifiers)", "Оператор All: Квантор загальності (∀)", 50)
    add_theory_card(s50, 0.8, 1.55, 5.72, 4.25,
        'Перевірка виконання умови для всіх елементів', ACCENT_INDIGO,
        [
            ('Суть All(predicate):', 'Оцінює кожен елемент колекції. Повертає true лише тоді, коли предикат виконується для ВСІХ без винятку елементів.'),
            ('Рання зупинка (Short-circuiting):', 'Як тільки трапляється хоча б ОДИН елемент з false, All негайно завершує роботу і повертає false.'),
            ('Пастка порожньої множини (Vacuous Truth):', 'Над порожньою колекцією All ЗАВЖДИ повертає true, оскільки контрприкладів не існує!'),
            ('Математичний еквівалент:', 'Позначає квантор загальності ∀x P(x). Дорівнює запереченню !Any(x => !P(x)).')
        ],
        '⚡ Практичний приклад:',
        'studentList.All(s => s.Age > 12 && s.Age < 20) поверне false, оскільки Moin (21) та Ram (20) старші за 19 років.'
    )
    add_code_panel(s50, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Перевірка: чи всі студенти є підлітками?',
        'bool areAllTeenAgers = studentList.All(',
        '    s => s.Age > 12 && s.Age < 20);',
        'Console.WriteLine($"Усі підлітки: {areAllTeenAgers}");',
        '// Output: Усі підлітки: False (Moin має вік 21)',
        '',
        '// 2. Перевірка: чи всі мають вік понад 10 років?',
        'bool allAbove10 = studentList.All(s => s.Age > 10);',
        'Console.WriteLine($"Усі старші 10: {allAbove10}");',
        '// Output: Усі старші 10: True',
        '',
        '// 3. Поведінка над порожньою колекцією (Vacuous Truth):',
        'var empty = Enumerable.Empty<Student>();',
        'bool emptyCheck = empty.All(s => s.Age > 100);',
        'Console.WriteLine($"Empty All: {emptyCheck}"); // True!',
        '',
        '// -> Roslyn генерує оптимізований цикл з раннім return false'
    ], 'Program.cs')
    add_bottom_banner(s50, "💡 Логічний аналог:",
                      "All(p) еквівалентний твердженню: !Any(x => !p(x)) (за законом подвійності де Моргана).")
    # ==========================================
    # SLIDE 51: ОПЕРАТОР ANY
    # ==========================================
    s51 = prs.slides.add_slide(blank_layout)
    add_bg(s51)
    add_header(s51, "10 · Квантори (Quantifiers)", "Оператор Any: Квантор існування (∃)", 51)
    add_theory_card(s51, 0.8, 1.55, 5.72, 4.25,
        'Перевірка наявності хоча б одного елемента', ACCENT_SKY,
        [
            ('1. Any() без параметрів:', 'Перевіряє, чи містить колекція хоча б один елемент за константний час O(1).'),
            ('2. Any(predicate) з умовою:', 'Перевіряє, чи існує хоча б один елемент, що задовольняє предикат.'),
            ('Миттєве переривання:', 'Як тільки знайдено перший true, ітерація негайно зупиняється (не читаючи решту).'),
            ('Порожні колекції:', 'На відміну від All, метод Any() над порожньою колекцією завжди повертає false.')
        ],
        '⚡ Золоте правило продуктивності:',
        'НІКОЛИ не пишіть if (list.Count() > 0) — це змушує рахувати всі елементи! Завжди пишіть if (list.Any()) за O(1) часу!'
    )
    add_code_panel(s51, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Чи є в списку хоча б один підліток?',
        'bool hasTeen = studentList.Any(s => s.Age > 12 && s.Age < 20);',
        'Console.WriteLine($"Є підліток: {hasTeen}");',
        '// Output: Є підліток: True (знайдено John на 1-му кроці!)',
        '',
        '// 2. Швидка перевірка на непорожність колекції O(1):',
        'if (studentList.Any())',
        '{',
        '    Console.WriteLine("Список студентів не порожній!");',
        '}',
        '',
        '// 3. Пошук неіснуючого елемента:',
        'bool hasSenior = studentList.Any(s => s.Age >= 60);',
        'Console.WriteLine($"Є пенсіонери: {hasSenior}"); // False',
        '',
        '// -> Any() негайно повертає true при першому збігу (Short-circuit)'
    ], 'Program.cs')
    add_bottom_banner(s51, "💡 Економія ресурсів:",
                      "У базах даних Any(predicate) транслюється у надшвидкий SQL: IF EXISTS (SELECT 1 FROM Students WHERE ...).")
    # ==========================================
    # SLIDE 52: AGGREGATION OPERATORS ВСТУП
    # ==========================================
    s52 = prs.slides.add_slide(blank_layout)
    add_bg(s52)
    add_header(s52, "11 · Агрегація (Aggregation)", "Оператори агрегації: Скалярні підсумки та математичні операції", 52)
    add_theory_card(s52, 0.8, 1.55, 5.72, 4.25,
        'Згортка послідовностей у скалярні значення', ACCENT_INDIGO,
        [
            ('Суть агрегації:', "Приймає послідовність елементів і згортає її в одне скалярне число або об'єкт."),
            ('Негайне виконання (Immediate):', 'Усі агрегатні оператори виконуються негайно, оскільки вимагають повного перегляду даних.'),
            ('Базовий набір BCL:', 'Aggregate (універсальний редюсер), Average, Count, LongCount, Max, Min, Sum.'),
            ("Константна пам'ять:", "Агрегати не накопичують елементи в пам'яті, обчислюючи результат в одному акумуляторі O(1).")
        ],
        '⚡ Пастка порожніх списків:',
        'Average, Min та Max викидають InvalidOperationException над порожніми числовими послідовностями!'
    )
    add_code_panel(s52, 6.81, 1.55, 5.72, 4.25, [
        'int[] numbers = { 10, 20, 30, 40, 50 };',
        '',
        'int totalCount = numbers.Count();     // 5',
        'int totalSum   = numbers.Sum();       // 150',
        'double avg     = numbers.Average();   // 30.0',
        'int minVal     = numbers.Min();       // 10',
        'int maxVal     = numbers.Max();       // 50',
        '',
        '// Кастомна агрегація (добуток усіх чисел):',
        'int product = numbers.Aggregate((a, b) => a * b);',
        '',
        'Console.WriteLine($"Count: {totalCount}, Sum: {totalSum}, Avg: {avg}");',
        'Console.WriteLine($"Min: {minVal}, Max: {maxVal}, Product: {product}");',
        '',
        '// -> Output:',
        '// -> Count: 5, Sum: 150, Avg: 30',
        '// -> Min: 10, Max: 50, Product: 12000000'
    ], 'Program.cs')
    add_bottom_banner(s52, "💡 Складність за часом:",
                      "Усі скалярні агрегати працюють за лінійний час O(N) з константною пам'яттю O(1).")
    # ==========================================
    # SLIDE 53: ТАБЛИЦЯ АГРЕГАТНИХ ОПЕРАТОРІВ
    # ==========================================
    s53 = prs.slides.add_slide(blank_layout)
    add_bg(s53)
    add_header(s53, "11 · Агрегація (Aggregation)", "Повна зведена таблиця операторів агрегації LINQ", 53)
    headers_53 = ["Метод LINQ", "Опис", "Що робить", "Повертає", "Приклад", "Результат"]
    rows_53 = [
        ["Aggregate", "Кастомна агрегація (множення, конкатенація).", "Послідовно накопичує результат через акумулятор.", "T / TResult", "[1, 2, 3, 4].Aggregate((a, b) => a * b)", "24"],
        ["Average", "Середнє арифметичне числових значень.", "Знаходить математичне середнє.", "double / decimal", "[1, 2, 3, 4].Average()", "2.5"],
        ["Count", "Кількість елементів у колекції.", "Рахує всі або ті, що відповідають умові.", "int", "[1, 2, 3, 4].Count(x => x > 2)", "2"],
        ["LongCount", "Кількість елементів для великих масивів.", "Використовується для колекцій розміром > 2 млрд.", "long", "[1, 2, 3].LongCount()", "3L"],
        ["Max", "Найбільше значення у колекції.", "Знаходить максимальний елемент або ключ.", "Тип елемента", "[1, 2, 5, 3].Max()", "5"],
        ["Min", "Найменше значення у колекції.", "Знаходить мінімальний елемент або ключ.", "Тип елемента", "[1, 2, 5, 3].Min()", "1"],
        ["Sum", "Сума всіх числових елементів.", "Додає всі елементи між собою.", "int / double / decimal", "[1, 2, 3, 4].Sum()", "10"]
    ]
    add_table_card(s53, 0.8, 1.55, 11.733, 4.25, headers_53, rows_53, [1.5, 2.5, 2.5, 1.4, 2.5, 1.333], ACCENT_INDIGO)
    add_bottom_banner(s53, "💡 Безпека типів:",
                      "Для безпечного обчислення Average на nullable списках використовуйте list.Cast<double?>().Average() ?? 0.0.")

    # ==========================================
    # SLIDE 54: ОПЕРАТОР AGGREGATE ВСТУП
    # ==========================================
    s54 = prs.slides.add_slide(blank_layout)
    add_bg(s54)
    add_header(s54, "11 · Агрегація (Aggregation)", "Оператор Aggregate: Функціональний редюсер (Fold / Reduce)", 54)
    add_theory_card(s54, 0.8, 1.55, 5.72, 4.25,
        'Анатомія накопичувальної операції (Accumulator)', ACCENT_INDIGO,
        [
            ('Універсальність Aggregate:', 'Будь-який інший агрегат (Sum, Count, Max) можна виразити через метод Aggregate.'),
            ('Акумуляторний крок:', 'Функція func(accumulated, nextItem) приймає накопичений проміжний результат та черговий елемент.'),
            ('Три перевантаження:', '1) Без seed (перший елемент є seed); 2) Із явним seed; 3) Із seed та фінальним проектором результату.'),
            ('Імутабельність накопичувача:', 'Рекомендовано використовувати чисті функції без зміни зовнішнього стану системи.')
        ],
        '⚡ Схема обчислення (a, b) => a + ", " + b :',
        "Крок 1: 'One' + ', ' + 'Two' -> 'One, Two' | Крок 2: 'One, Two' + ', ' + 'Three'..."
    )
    add_code_panel(s54, 6.81, 1.55, 5.72, 4.25, [
        "// 1. Базовий приклад: об'єднання рядків через кому",
        'IList<string> strList = new List<string>() {',
        '    "One", "Two", "Three", "Four", "Five"',
        '};',
        '',
        'string commaSeparated =',
        '    strList.Aggregate((s1, s2) => s1 + ", " + s2);',
        'Console.WriteLine(commaSeparated);',
        '// Output: One, Two, Three, Four, Five',
        '',
        '// 2. Математичний підрахунок факторіала 5!:',
        'int factorial = Enumerable.Range(1, 5)',
        '    .Aggregate((acc, n) => acc * n);',
        'Console.WriteLine($"5! = {factorial}"); // 120',
        '',
        '// 3. Знаходження суми квадратів чисел:',
        'int sumSquares = new[] { 1, 2, 3, 4 }.Aggregate(0, (acc, n) => acc + n * n);',
        'Console.WriteLine($"Сума квадратів: {sumSquares}"); // 30'
    ], 'Program.cs')
    add_bottom_banner(s54, "💡 Важлива деталь:",
                      "Якщо послідовність містить лише один елемент, метод Aggregate(func) поверне його без виклику функції.")
    # ==========================================
    # SLIDE 55: ПРИКЛАД AGGREGATE У METHOD SYNTAX
    # ==========================================
    s55 = prs.slides.add_slide(blank_layout)
    add_bg(s55)
    add_header(s55, "11 · Агрегація (Aggregation)", "Практика Aggregate: Конкатенація рядків без зайвих виділень", 55)
    add_theory_card(s55, 0.8, 1.55, 5.72, 4.25,
        "Покрокове накопичення результату", ACCENT_SKY,
        [
            ("Вхідний список:", "strList містить елементи 'One', 'Two', 'Three', 'Four', 'Five'."),
            ("Крок 1:", "s1 = 'One', s2 = 'Two' -> повертає 'One, Two'."),
            ("Крок 2:", "s1 = 'One, Two', s2 = 'Three' -> повертає 'One, Two, Three'."),
            ("Фінал:", "Повертає рядок 'One, Two, Three, Four, Five'.")
        ],
        "⚡ Порада щодо оптимізації:",
        "Для мільйонів рядків використовуйте StringBuilder у версії Aggregate зі seed, щоб не створювати проміжних рядків у Heap!"
    )
    add_code_panel(s55, 6.81, 1.55, 5.72, 4.25, [
        "IList<string> strList = new List<string>() {",
        "    \"One\", \"Two\", \"Three\", \"Four\", \"Five\"",
        "};",
        "",
        "var commaSeparatedString = strList.Aggregate(",
        "    (s1, s2) => s1 + \", \" + s2);",
        "",
        "Console.WriteLine(commaSeparatedString);",
        "// Output: One, Two, Three, Four, Five",
        "",
        "// Високопродуктивний варіант зі StringBuilder:",
        "var sbResult = strList.Aggregate(",
        "    new StringBuilder(),",
        "    (sb, s) => sb.Append(s).Append(\", \"),",
        "    sb => sb.ToString().TrimEnd(',', ' '));"
    ])
    add_bottom_banner(s55, "💡 Зручність C#:",
                      "Aggregate дозволяє будувати складні кумулятивні звіти без ручних циклів for та тимчасових змінних.")

    # ==========================================
    # SLIDE 56: AGGREGATE ЗІ ЗНАЧЕННЯМ SEED
    # ==========================================
    s56 = prs.slides.add_slide(blank_layout)
    add_bg(s56)
    add_header(s56, "11 · Агрегація (Aggregation)", "Перевантаження Aggregate із початковим значенням (Seed)", 56)
    add_theory_card(s56, 0.8, 1.55, 5.72, 4.25,
        'Агрегація з явним початковим акумулятором', ACCENT_INDIGO,
        [
            ('Призначення Seed:', 'Задає стартове значення для акумулятора до початку перегляду елементів колекції.'),
            ('Зміна типу акумулятора:', 'Дозволяє акумулятору мати тип, відмінний від типу елементів колекції (наприклад, із Student у string).'),
            ('Безпека порожнього списку:', 'Якщо колекція порожня, Aggregate(seed, func) безпечно поверне seed без винятку!'),
            ('Фінальний селектор:', 'Третє перевантаження приймає resultSelector для постобробки результату акумулятора.')
        ],
        '⚡ Сигнатура методу:',
        'Aggregate<TSource, TAccumulate>(seed, (acc, item) => ..., resultSelector)'
    )
    add_code_panel(s56, 6.81, 1.55, 5.72, 4.25, [
        '// Студенти для згортки в текстовий рядок:',
        'IList<Student> studentList = new List<Student>() {',
        '    new Student() { StudentID = 1, StudentName = "John", Age = 13 },',
        '    new Student() { StudentID = 2, StudentName = "Moin", Age = 21 },',
        '    new Student() { StudentID = 3, StudentName = "Bill", Age = 18 }',
        '};',
        '',
        '// Агрегація зі Seed значенням "Student Names: ":',
        'string studentNames = studentList.Aggregate<Student, string>(',
        '    "Student Names: ",',
        '    (str, s) => str += s.StudentName + ", ");',
        '',
        'Console.WriteLine(studentNames);',
        '// -> Output: Student Names: John, Moin, Bill, ',
        '',
        '// Агрегація у число з початковим балансом 100:',
        'int totalAgeWithBase = studentList.Aggregate(100, (acc, s) => acc + s.Age);',
        'Console.WriteLine($"Баланс з віком: {totalAgeWithBase}"); // 152'
    ], 'Program.cs')
    add_bottom_banner(s56, "💡 Фінальний селектор:",
                      "Перевантаження з resultSelector дозволяє на фініші видалити зайву кому: str => str.TrimEnd(',', ' ').")
    # ==========================================
    # SLIDE 57: МЕТОД AVERAGE
    # ==========================================
    s57 = prs.slides.add_slide(blank_layout)
    add_bg(s57)
    add_header(s57, "11 · Агрегація (Aggregation)", "Метод Average: Середнє арифметичне чисел та об'єктів", 57)
    add_dual_comparison_card(s57, 0.8, 1.55, 5.72, 4.25,
        "Average() для числових списків", ACCENT_INDIGO,
        [
            ("Простий список:", "Обчислює середнє безпосередньо для списку чисел intList."),
            ("Тип результату:", "Завжди повертає число з плаваючою крапкою double або decimal."),
            ("Формула:", "Сума всіх чисел, поділена на їхню кількість (150 / 5 = 30.0).")
        ],
        [
            "IList<int> intList = new List<int>() {",
            "    10, 20, 30, 40, 50",
            "};",
            "",
            "var avg = intList.Average();",
            "Console.WriteLine(avg);",
            "// Output: 30"
        ]
    )
    add_dual_comparison_card(s57, 6.81, 1.55, 5.72, 4.25,
        "Average(selector) для об'єктів", ACCENT_EMERALD,
        [
            ("Лямбда-селектор:", "s => s.Age вказує числову властивість, за якою рахується середнє."),
            ("Комбінація з Where:", "Можна обчислити середній вік лише для певної підгрупи (наприклад, підлітків)."),
            ("Точність:", "Запобігає помилкам цілочисельного ділення C#.")
        ],
        [
            "// Середній вік усіх студентів:",
            "var avgAge = studentList",
            "    .Average(s => s.Age);",
            "",
            "// Середній вік підлітків:",
            "var avgTeenAge = studentList",
            "    .Where(s => s.Age < 20)",
            "    .Average(s => s.Age);",
            "Console.WriteLine(avgAge); // 17.4"
        ]
    )
    add_bottom_banner(s57, "💡 Увага:",
                      "Виклик .Average() на порожньому списку кине InvalidOperationException! Завжди перевіряйте наявність елементів.")

    # ==========================================
    # SLIDE 58: МЕТОД COUNT ТА COUNT З ПРЕДИКАТОМ
    # ==========================================
    s58 = prs.slides.add_slide(blank_layout)
    add_bg(s58)
    add_header(s58, "11 · Агрегація (Aggregation)", "Метод Count(): Підрахунок загальної кількості та за умовою", 58)
    add_dual_comparison_card(s58, 0.8, 1.55, 5.72, 4.25,
        "Count() без предикату", ACCENT_INDIGO,
        [
            ("Підрахунок елементів:", "Повертає загальну кількість елементів у колекції."),
            ("Оптимізація ICollection:", "Якщо колекція реалізує ICollection, повертає властивість Count без повного перебору!"),
            ("Тип повернення:", "Повертає 32-бітне ціле число int.")
        ],
        [
            "IList<int> intList = new List<int>() {",
            "    10, 21, 30, 45, 50, 87",
            "};",
            "",
            "var totalElements = intList.Count();",
            "Console.WriteLine(totalElements);",
            "// Output: 6"
        ]
    )
    add_dual_comparison_card(s58, 6.81, 1.55, 5.72, 4.25,
        "Count(predicate) з умовою", ACCENT_SKY,
        [
            ("Підрахунок з фільтром:", "Рахує лише ті елементи, для яких умова повертає true."),
            ("Еквівалентність:", "intList.Count(p) працює швидше і чистіше, ніж intList.Where(p).Count()."),
            ("Підрахунок парних:", "i => i % 2 == 0 рахує числа 10, 30, 50.")
        ],
        [
            "// Підрахунок парних чисел:",
            "var evenElements = intList",
            "    .Count(i => i % 2 == 0);",
            "Console.WriteLine(evenElements);",
            "// Output: 3",
            "",
            "// Студенти старше 18:",
            "var adults = studentList.Count(s => s.Age >= 18);"
        ]
    )
    add_bottom_banner(s58, "💡 LongCount:",
                      "Якщо даних більше 2 мільярдів записів (переповнення int.MaxValue), використовуйте метод LongCount(), що повертає long.")

    # ==========================================
    # SLIDE 59: МЕТОД MAX
    # ==========================================
    s59 = prs.slides.add_slide(blank_layout)
    add_bg(s59)
    add_header(s59, "11 · Агрегація (Aggregation)", "Метод Max: Знаходження найбільшого значення", 59)
    add_theory_card(s59, 0.8, 1.55, 5.72, 4.25,
        'Пошук максимального елемента або властивості', ACCENT_INDIGO,
        [
            ('Max() над числами:', 'Знаходить найбільше число у послідовності intList (серед 10, 21, 30, 45, 50, 87 це 87).'),
            ("Max(selector) над об'єктами:", 'Повертає максимальне значення вибраної числової властивості (найбільший вік серед студентів).'),
            ('Комбінація з фільтрацією:', 'Можна шукати максимум серед відфільтрованої підгрупи (найстарший серед підлітків).'),
            ('Новинка MaxBy (.NET 6+):', "Замість максимального числа повертає весь об'єкт студента з найбільшим віком!")
        ],
        '⚡ Відмінність Max від MaxBy (.NET 6+):',
        "Max(s => s.Age) повертає лише ЧИСЛО (21). Новий метод MaxBy(s => s.Age) повертає ЦІЛИЙ ОБ'ЄКТ студента!"
    )
    add_code_panel(s59, 6.81, 1.55, 5.72, 4.25, [
        'IList<int> intList = new List<int>() {',
        '    10, 21, 30, 45, 50, 87',
        '};',
        '',
        'var largestNumber = intList.Max();',
        'Console.WriteLine($"Max int: {largestNumber}"); // 87',
        '',
        'var largestEven = intList.Where(i => i % 2 == 0).Max();',
        'Console.WriteLine($"Max even: {largestEven}"); // 50',
        '',
        '// Найстарший студент (повертає число віку):',
        'var oldestAge = studentList.Max(s => s.Age);',
        'Console.WriteLine($"Oldest age: {oldestAge}"); // 21',
        '',
        "// Знаходження цілого об'єкта студента через MaxBy (.NET 6+):",
        'var oldestStudent = studentList.MaxBy(s => s.Age);',
        'Console.WriteLine($"Oldest: {oldestStudent.StudentName}"); // Moin'
    ], 'Program.cs')
    add_bottom_banner(s59, "💡 Аналогічний Min:",
                      "Метод Min() працює абсолютно ідентично, але знаходить найменше значення (наприклад, intList.Min() поверне 10).")
    # ==========================================
    # SLIDE 60: МЕТОД SUM
    # ==========================================
    s60 = prs.slides.add_slide(blank_layout)
    add_bg(s60)
    add_header(s60, "11 · Агрегація (Aggregation)", "Метод Sum: Підсумовування числових значень", 60)
    add_theory_card(s60, 0.8, 1.55, 5.72, 4.25,
        'Обчислення суми елементів колекції', ACCENT_SKY,
        [
            ('Числова сума:', 'Додає всі елементи між собою. Підтримує типи int, long, float, double, decimal.'),
            ('Селектор властивості:', 'studentList.Sum(s => s.Age) обчислює сумарний вік усіх студентів у групі.'),
            ('Безпека порожнього списку:', 'На відміну від Average/Min/Max, метод Sum() над порожнім списком повертає 0 (без винятку!).'),
            ('Переповнення (Overflow):', 'Якщо сума int може перевищити 2 млрд, рекомендується приводити числа до long.')
        ],
        '⚡ Практичне застосування в Enterprise:',
        'Підрахунок загальної вартості кошика замовлень: orders.Sum(o => o.UnitPrice * o.Quantity).'
    )
    add_code_panel(s60, 6.81, 1.55, 5.72, 4.25, [
        'IList<int> intList = new List<int>() {',
        '    10, 21, 30, 45, 50, 87',
        '};',
        '',
        'var totalSum = intList.Sum();',
        'Console.WriteLine($"Загальна сума: {totalSum}"); // 243',
        '',
        'var sumOfEvens = intList.Where(i => i % 2 == 0).Sum();',
        'Console.WriteLine($"Сума парних: {sumOfEvens}"); // 90',
        '',
        '// Сума віку підлітків:',
        'var teenAgeSum = studentList',
        '    .Where(s => s.Age < 20)',
        '    .Sum(s => s.Age);',
        'Console.WriteLine($"Сума віку підлітків: {teenAgeSum}"); // 46'
    ], 'Program.cs')
    add_bottom_banner(s60, "💡 Переповнення чисел:",
                      "Якщо сума може перевищити int.MaxValue, попередньо приведіть числа: numbers.Select(x => (long)x).Sum().")
    # ==========================================
    # SLIDE 61: ELEMENT OPERATORS ВСТУП
    # ==========================================
    s61 = prs.slides.add_slide(blank_layout)
    add_bg(s61)
    add_header(s61, "12 · Елементні оператори (Element)", "Елементні оператори: ElementAt та ElementAtOrDefault", 61)
    add_theory_card(s61, 0.8, 1.55, 5.72, 4.25,
        'Доступ до конкретного елемента за індексом', ACCENT_INDIGO,
        [
            ('Суть ElementAt:', 'Повертає елемент послідовності за вказаним числовим індексом (нумерація з 0).'),
            ('Оптимізація IList:', 'Якщо джерело реалізує IList<T>, доступ виконується миттєво O(1). Для потоку — перебір до потрібного кроку.'),
            ('Ризик винятку:', "ElementAt викидає ArgumentOutOfRangeException, якщо індекс від'ємний або більший за розмір списку."),
            ('Безпечний ElementAtOrDefault:', 'Повертає значення за замовчуванням (0, null), якщо індекс виходить за межі.')
        ],
        '⚡ Застосування:',
        'intList.ElementAt(1) повертає другий елемент. intList.ElementAtOrDefault(99) безпечно поверне 0.'
    )
    add_code_panel(s61, 6.81, 1.55, 5.72, 4.25, [
        'IList<int> intList = new List<int>() { 10, 20, 30, 40, 50 };',
        '',
        '// 1. Прямий доступ за індексом:',
        'int second = intList.ElementAt(1);',
        'Console.WriteLine($"Element at index 1: {second}"); // 20',
        '',
        '// 2. Безпечний доступ поза межами колекції:',
        'int outOfBounds = intList.ElementAtOrDefault(9);',
        'Console.WriteLine($"Element at index 9: {outOfBounds}"); // 0',
        '',
        '// 3. З рядковими колекціями:',
        'IList<string> strList = new List<string>() { "One", "Two", "Three" };',
        'string safeStr = strList.ElementAtOrDefault(10);',
        'Console.WriteLine($"String at index 10: {safeStr ?? "NULL"}"); // NULL',
        '',
        '// 4. Доступ до першого та останнього елементів:',
        'int first = intList.ElementAt(0); // 10',
        'int last = intList.ElementAt(intList.Count - 1); // 50'
    ], 'Program.cs')
    add_bottom_banner(s61, "💡 Рекомендація:",
                      "Для звичайних масивів та List<T> краще використовувати рідний індексатор arr[i]. ElementAt потрібен для довільних IEnumerable<T>.")
    # ==========================================
    # SLIDE 62: ТАБЛИЦЯ ЕЛЕМЕНТНИХ ОПЕРАТОРІВ
    # ==========================================
    s62 = prs.slides.add_slide(blank_layout)
    add_bg(s62)
    add_header(s62, "12 · Елементні оператори (Element)", "Повна зведена таблиця елементних операторів LINQ", 62)
    headers_62 = ["Метод LINQ", "Опис", "Що робить", "Приклад", "Результат"]
    rows_62 = [
        ["ElementAt", "Повертає елемент за індексом.", "Доступ до конкретного елемента за порядковим номером.", "[10, 20, 30].ElementAt(1)", "20"],
        ["ElementAtOrDefault", "Безпечний доступ за індексом.", "Повертає default, якщо індекс поза межами.", "[10, 20, 30].ElementAtOrDefault(5)", "0"],
        ["First", "Повертає перший елемент або перший за умовою.", "Кидає виняток, якщо нічого не знайдено.", "[1, 2, 3].First(x => x > 1)", "2"],
        ["FirstOrDefault", "Безпечний пошук першого елемента.", "Повертає default, якщо збігів немає.", "[1, 2, 3].FirstOrDefault(x => x > 5)", "0"],
        ["Last", "Повертає останній елемент або останній за умовою.", "Кидає виняток, якщо колекція порожня.", "[1, 2, 3].Last(x => x < 3)", "2"],
        ["LastOrDefault", "Безпечний пошук останнього елемента.", "Повертає default, якщо збігів немає.", "[1, 2, 3].LastOrDefault(x => x > 5)", "0"],
        ["Single", "Повертає ЄДИНИЙ елемент колекції.", "Кидає виняток, якщо елементів 0 або > 1.", "[5].Single()", "5"],
        ["SingleOrDefault", "Єдиний елемент або default.", "Кидає виняток, якщо знайдено > 1 елемента.", "[5].SingleOrDefault()", "5"]
    ]
    add_table_card(s62, 0.8, 1.55, 11.733, 4.25, headers_62, rows_62, [1.8, 2.5, 3.2, 2.8, 1.433], ACCENT_INDIGO)
    add_bottom_banner(s62, "💡 Золоте правило суфікса OrDefault:",
                      "Усі методи з OrDefault ніколи не викидають виняток при відсутності елементів, а повертають null або 0.")

    # ==========================================
    # SLIDE 63: ПРАКТИКА ELEMENTAT ТА ELEMENTATORDEFAULT
    # ==========================================
    s63 = prs.slides.add_slide(blank_layout)
    add_bg(s63)
    add_header(s63, "12 · Елементні оператори (Element)", "Практичний приклад: ElementAt та ElementAtOrDefault у C#", 63)
    add_dual_comparison_card(s63, 0.8, 1.55, 5.72, 4.25,
        "Числові масиви (int)", ACCENT_INDIGO,
        [
            ("ElementAt(0):", "Повертає перший елемент (10)."),
            ("ElementAt(2):", "Повертає третій елемент (30)."),
            ("ElementAtOrDefault(9):", "Індекс 9 поза межами — повертає число 0.")
        ],
        [
            "IList<int> intList = new List<int>() { 10, 20, 30, 40, 50 };",
            "",
            "Console.WriteLine(intList.ElementAt(0)); // 10",
            "Console.WriteLine(intList.ElementAt(2)); // 30",
            "Console.WriteLine(intList.ElementAtOrDefault(9)); // 0"
        ]
    )
    add_dual_comparison_card(s63, 6.81, 1.55, 5.72, 4.25,
        "Рядкові масиви (string)", ACCENT_SKY,
        [
            ("ElementAt(0):", "Повертає рядок 'One'."),
            ("ElementAt(1):", "Повертає рядок 'Two'."),
            ("ElementAtOrDefault(9):", "Індекс 9 поза межами — безпечно повертає null (порожньо).")
        ],
        [
            "IList<string> strList = new List<string>() {",
            "    \"One\", \"Two\", \"Three\", \"Four\", \"Five\"",
            "};",
            "",
            "Console.WriteLine(strList.ElementAt(0)); // One",
            "Console.WriteLine(strList.ElementAt(1)); // Two",
            "Console.WriteLine(strList.ElementAtOrDefault(9)); // null"
        ]
    )
    add_bottom_banner(s63, "💡 Пастка винятку:",
                      "Виклик intList.ElementAt(9) викине ArgumentOutOfRangeException! Для неперевірених індексів завжди використовуйте ElementAtOrDefault.")

    # ==========================================
    # SLIDE 64: FIRST ТА FIRSTORDEFAULT ТАБЛИЦЯ
    # ==========================================
    s64 = prs.slides.add_slide(blank_layout)
    add_bg(s64)
    add_header(s64, "12 · Елементні оператори (Element)", "Порівняння First та FirstOrDefault", 64)
    headers_64 = ["Метод LINQ", "Опис", "Що робить", "Приклад", "Результат"]
    rows_64 = [
        ["First", "Повертає перший елемент колекції або перший за умовою.", "Якщо елементів немає або жоден не відповідає умові — кидає InvalidOperationException.", "[1, 2, 3].First(x => x > 1)", "2"],
        ["FirstOrDefault", "Повертає перший елемент або перший за умовою.", "Якщо елементів немає або жоден не відповідає умові — повертає значення за замовчуванням (default).", "[1, 2, 3].FirstOrDefault(x => x > 5)", "0 (для int)"]
    ]
    add_table_card(s64, 0.8, 1.55, 11.733, 1.85, headers_64, rows_64, [2.0, 3.0, 3.2, 2.5, 1.033], ACCENT_INDIGO)
    add_code_panel(s64, 0.8, 3.55, 11.733, 2.25, [
        'IList<int> numbers = new List<int>() { 10, 25, 30, 45, 50 };',
        'int firstEven = numbers.First(n => n % 2 == 0);                    // 10 (знайдено одразу)',
        'int safeMatch = numbers.FirstOrDefault(n => n > 100);              // 0 (безпечно default)',
        'int withDef   = numbers.FirstOrDefault(n => n > 100, -1);          // -1 (кастомний default)',
        'Console.WriteLine($"FirstEven: {firstEven}, SafeMatch: {safeMatch}, WithDef: {withDef}");',
        '// Output: FirstEven: 10, SafeMatch: 0, WithDef: -1'
    ], 'FirstVsFirstOrDefault.cs')
    add_bottom_banner(s64, "💡 Кастомне дефолтне значення (.NET 6+):",
                      "У сучасному C# можна передавати власне значення за замовчуванням: list.FirstOrDefault(x => x > 5, defaultValue: -1).")
    # ==========================================
    # SLIDE 65: ПРИКЛАД МЕТОДУ FIRST
    # ==========================================
    s65 = prs.slides.add_slide(blank_layout)
    add_bg(s65)
    add_header(s65, "12 · Елементні оператори (Element)", "Практичний приклад методу First() у C#", 65)
    add_theory_card(s65, 0.8, 1.55, 5.72, 4.25,
        'Поведінка First() при збігах та на порожніх даних', ACCENT_INDIGO,
        [
            ('intList.First():', 'Повертає перший елемент масиву (число 7).'),
            ('intList.First(i => i % 2 == 0):', 'Повертає перший парний елемент (число 10).'),
            ('emptyList.First():', 'Викидає виняток InvalidOperationException: Sequence contains no elements!'),
            ('Зупинка конвеєра:', 'Метод First() завершує сканування негайно при знаходженні першого результату.')
        ],
        '⚡ Чому First кидає помилку:',
        "First вимагає обов'язкової наявності хоча б одного елемента. Якщо бізнес-логіка допускає відсутність даних, використовуйте FirstOrDefault."
    )
    add_code_panel(s65, 6.81, 1.55, 5.72, 4.25, [
        'IList<int> intList = new List<int>() { 7, 10, 21, 30, 45, 50, 87 };',
        'IList<string> strList = new List<string>() { null, "Two", "Three" };',
        'IList<string> emptyList = new List<string>();',
        '',
        'int firstNum = intList.First(); // 7',
        'int firstEven = intList.First(i => i % 2 == 0); // 10',
        'string firstStr = strList.First(); // null',
        '',
        'Console.WriteLine($"First: {firstNum}, FirstEven: {firstEven}");',
        '',
        '// Спроба виклику на порожньому списку кине помилку:',
        'try {',
        '    emptyList.First();',
        '} catch (InvalidOperationException ex) {',
        '    Console.WriteLine($"Очікуваний виняток: {ex.Message}");',
        '}',
        '// Output: Sequence contains no elements'
    ], 'Program.cs')
    add_bottom_banner(s65, "💡 Short-circuiting:",
                      "Метод First() завершує роботу на першому ж знайденому елементі і не перебирає залишок колекції.")
    # ==========================================
    # SLIDE 66: ПРИКЛАД FIRSTORDEFAULT
    # ==========================================
    s66 = prs.slides.add_slide(blank_layout)
    add_bg(s66)
    add_header(s66, "12 · Елементні оператори (Element)", "Практичний приклад FirstOrDefault() у C#", 66)
    add_theory_card(s66, 0.8, 1.55, 5.72, 4.25,
        'Безпечний пошук без ризику падіння програми', ACCENT_SKY,
        [
            ('intList.FirstOrDefault():', 'Повертає перший елемент (7).'),
            ('intList.FirstOrDefault(i => i % 2 == 0):', 'Повертає перший парний елемент (10).'),
            ('emptyList.FirstOrDefault():', 'Безпечно повертає null (для посилань) або 0 (для чисел) без аварії програми.'),
            ('Перевантаження defaultValue:', 'У .NET 6+ дозволяє передавати власне значення за замовчуванням, якщо нічого не знайдено.')
        ],
        '⚡ Рекомендація для веб-сервісів:',
        'У контролерах Web API завжди шукайте сутності через FirstOrDefault(x => x.Id == id) та повертайте NotFound(), якщо результат == null.'
    )
    add_code_panel(s66, 6.81, 1.55, 5.72, 4.25, [
        'IList<int> intList = new List<int>() { 7, 10, 21, 30, 45, 50, 87 };',
        'IList<string> emptyList = new List<string>();',
        '',
        'int first = intList.FirstOrDefault(); // 7',
        'int firstEven = intList.FirstOrDefault(i => i % 2 == 0); // 10',
        '',
        '// Безпечне виконання над порожньою колекцією:',
        'string res = emptyList.FirstOrDefault();',
        'Console.WriteLine($"Результат: {res ?? "[NULL]"}"); // [NULL]',
        '',
        '// 2. Пошук з кастомним дефолтним значенням (.NET 6+):',
        'int notFound = intList.FirstOrDefault(i => i > 1000, -1);',
        'Console.WriteLine($"Не знайдено: {notFound}"); // -1',
        '',
        '// 3. Безпечний пошук сутності:',
        'Student std = studentList.FirstOrDefault(s => s.StudentID == 99);'
    ], 'Program.cs')
    add_bottom_banner(s66, "💡 Порада з безпеки:",
                      "Якщо default(T) для int є 0, а 0 є валідним бізнес-значенням, передавайте кастомний default: list.FirstOrDefault(p, -1).")
    # ==========================================
    # SLIDE 67: LAST ТА LASTORDEFAULT ТАБЛИЦЯ
    # ==========================================
    s67 = prs.slides.add_slide(blank_layout)
    add_bg(s67)
    add_header(s67, "12 · Елементні оператори (Element)", "Оператори Last та LastOrDefault", 67)
    headers_67 = ["Метод LINQ", "Опис", "Що робить", "Приклад", "Результат"]
    rows_67 = [
        ["Last", "Повертає останній елемент колекції або останній за умовою.", "Якщо немає підходящих елементів — кидає виняток.", "[1, 2, 3, 4].Last(x => x < 4)", "3"],
        ["LastOrDefault", "Повертає останній елемент або останній за умовою.", "Якщо немає підходящих елементів — повертає default.", "[1, 2, 3, 4].LastOrDefault(x => x > 5)", "0 (для int)"]
    ]
    add_table_card(s67, 0.8, 1.55, 11.733, 1.85, headers_67, rows_67, [2.0, 3.0, 3.2, 2.5, 1.033], ACCENT_INDIGO)
    add_code_panel(s67, 0.8, 3.55, 11.733, 2.25, [
        'IList<int> numbers = new List<int>() { 10, 21, 30, 45, 50, 87 };',
        'int lastNum  = numbers.Last();                                     // 87 (останнє число)',
        'int lastEven = numbers.Last(n => n % 2 == 0);                      // 50 (останнє парне)',
        'int notFound = numbers.LastOrDefault(n => n > 100);                // 0 (безпечно default)',
        'Console.WriteLine($"Last: {lastNum}, LastEven: {lastEven}, NotFound: {notFound}");',
        '// Output: Last: 87, LastEven: 50, NotFound: 0'
    ], 'LastVsLastOrDefault.cs')
    add_bottom_banner(s67, "💡 Продуктивність Last():",
                      "Якщо колекція є потоком (не масивом), Last змушений прочитати ВСЮ колекцію до самого кінця, щоб знайти останній елемент.")
    # ==========================================
    # SLIDE 68: ПРИКЛАД МЕТОДУ LAST
    # ==========================================
    s68 = prs.slides.add_slide(blank_layout)
    add_bg(s68)
    add_header(s68, "12 · Елементні оператори (Element)", "Практичний приклад методу Last() у C#", 68)
    add_theory_card(s68, 0.8, 1.55, 5.72, 4.25,
        'Пошук останнього елемента у списку', ACCENT_INDIGO,
        [
            ('intList.Last():', 'Повертає останній елемент масиву (число 87).'),
            ('intList.Last(i => i % 2 == 0):', 'Повертає останній парний елемент масиву (число 50).'),
            ('strList.Last():', "Повертає останній рядок списку ('Five')."),
            ('emptyList.Last():', 'Викидає виняток InvalidOperationException на порожній колекції.')
        ],
        '⚡ Оптимізація IList:',
        'Для List<T> або T[] метод Last() миттєво бере елемент за індексом list[list.Count - 1] за O(1) часу.'
    )
    add_code_panel(s68, 6.81, 1.55, 5.72, 4.25, [
        'IList<int> intList = new List<int>() { 7, 10, 21, 30, 45, 50, 87 };',
        'IList<string> strList = new List<string>() { null, "Two", "Three", "Four", "Five" };',
        '',
        'int lastInt = intList.Last(); // 87',
        'int lastEven = intList.Last(i => i % 2 == 0); // 50',
        'string lastStr = strList.Last(); // Five',
        '',
        'Console.WriteLine($"Last: {lastInt}, LastEven: {lastEven}, LastStr: {lastStr}");',
        '',
        '// Спроба викликати Last на порожньому списку кине виняток:',
        'var empty = new List<int>();',
        'try {',
        '    empty.Last();',
        '} catch (InvalidOperationException ex) {',
        '    Console.WriteLine($"Помилка: {ex.Message}");',
        '}'
    ], 'Program.cs')
    add_bottom_banner(s68, "💡 Важлива відмінність:",
                      "First() може завершитися на 1-му кроці, але Last(predicate) завжди змушений сканувати колекцію до кінця.")
    # ==========================================
    # SLIDE 69: ПРИКЛАД LASTORDEFAULT
    # ==========================================
    s69 = prs.slides.add_slide(blank_layout)
    add_bg(s69)
    add_header(s69, "12 · Елементні оператори (Element)", "Практичний приклад LastOrDefault() у C#", 69)
    add_theory_card(s69, 0.8, 1.55, 5.72, 4.25,
        'Безпечний пошук останнього елемента', ACCENT_SKY,
        [
            ('intList.LastOrDefault():', 'Повертає останній елемент (87).'),
            ('intList.LastOrDefault(i => i % 2 == 0):', 'Повертає останній парний елемент (50).'),
            ('emptyList.LastOrDefault():', 'Безпечно повертає 0 або null без винятку.'),
            ('Пошук транзакцій:', 'Ідеально для знаходження останньої виконаної дії або останнього запису в журналі аудиту.')
        ],
        '⚡ Сфера застосування:',
        'Пошук останньої виконаної транзакції користувача або останнього доданого запису в лог-файл.'
    )
    add_code_panel(s69, 6.81, 1.55, 5.72, 4.25, [
        'IList<int> intList = new List<int>() { 7, 10, 21, 30, 45, 50, 87 };',
        'IList<string> emptyList = new List<string>();',
        '',
        'int last = intList.LastOrDefault(); // 87',
        'int lastEven = intList.LastOrDefault(i => i % 2 == 0); // 50',
        '',
        '// Безпечне виконання над порожнім списком:',
        'var lastEmpty = emptyList.LastOrDefault();',
        'Console.WriteLine(lastEmpty == null ? "NULL" : lastEmpty); // NULL',
        '',
        '// 2. Пошук з умовою без збігів:',
        'int noMatch = intList.LastOrDefault(i => i > 500);',
        'Console.WriteLine($"No match: {noMatch}"); // 0',
        '',
        '// 3. Кастомний default у .NET 6+:',
        'int defMatch = intList.LastOrDefault(i => i > 500, -1); // -1'
    ], 'Program.cs')
    add_bottom_banner(s69, "💡 Порада:",
                      "Якщо потрібен останній елемент відсортованої бази даних, ефективніше зробити .OrderByDescending().FirstOrDefault().")
    # ==========================================
    # SLIDE 70: ПРИКЛАД ВІДСУТНОСТІ ЗБІГІВ LAST
    # ==========================================
    s70 = prs.slides.add_slide(blank_layout)
    add_bg(s70)
    add_header(s70, "12 · Елементні оператори (Element)", "Поведінка Last проти LastOrDefault при відсутності збігів", 70)
    add_theory_card(s70, 0.8, 1.55, 5.72, 4.25,
        'Коли умова фільтра не знаходить жодного елемента', ACCENT_CRIMSON,
        [
            ('intList.Last(i => i > 250):', 'Жодне число в масиві не перевищує 250.'),
            ('Реакція Last():', 'Викидає InvalidOperationException: Sequence contains no matching element!'),
            ('Реакція LastOrDefault():', 'Безпечно повертає значення default (0 або null) без аварії.'),
            ('Діагностика продакшену:', "Помилка 'Sequence contains no matching element' є маркером використання Last замість LastOrDefault.")
        ],
        '⚡ Золоте правило стабільності:',
        'Якщо відповідність предикату не гарантована на 100%, завжди використовуйте LastOrDefault() з наступною перевіркою результату.'
    )
    add_code_panel(s70, 6.81, 1.55, 5.72, 4.25, [
        'IList<int> intList = new List<int>() { 7, 10, 21, 30, 45, 50, 87 };',
        'IList<string> strList = new List<string>() { null, "Two", "Three", "Four", "Five" };',
        '',
        '// 1. Last з умовою без збігів КИДАЄ ВИЙНЯТОК:',
        '// intList.Last(i => i > 250);',
        '// -> Run-time exception: Sequence contains no matching element',
        '',
        '// 2. LastOrDefault з умовою без збігів БЕЗПЕЧНИЙ:',
        'int notFound = intList.LastOrDefault(i => i > 250);',
        'Console.WriteLine($"Не знайдено: {notFound}"); // 0',
        '',
        "var strFound = strList.LastOrDefault(s => s != null && s.Contains('T'));",
        'Console.WriteLine($"Знайдено рядок: {strFound}"); // Three',
        '',
        '// 3. З кастомним default значенням (.NET 6+):',
        'int safeCustom = intList.LastOrDefault(i => i > 500, defaultValue: -1);',
        'Console.WriteLine($"Кастомний результат: {safeCustom}"); // -1'
    ], 'Program.cs')
    add_bottom_banner(s70, "💡 Діагностика помилок:",
                      "Помилка 'Sequence contains no matching element' є типовим маркером некоректного використання First()/Last() замість OrDefault.")
    # ==========================================
    # SLIDE 71: SINGLE ТА SINGLEORDEFAULT ТАБЛИЦЯ
    # ==========================================
    s71 = prs.slides.add_slide(blank_layout)
    add_bg(s71)
    add_header(s71, "12 · Елементні оператори (Element)", "Оператори Single та SingleOrDefault: Перевірка унікальності", 71)
    headers_71 = ["Метод LINQ", "Опис", "Що робить", "Приклад", "Результат"]
    rows_71 = [
        ["Single", "Повертає ЄДИНИЙ елемент колекції або єдиний за умовою.", "Якщо елементів немає або їх більше одного — кидає InvalidOperationException.", "[5].Single()", "5"],
        ["SingleOrDefault", "Повертає єдиний елемент або значення default.", "Якщо елементів немає — повертає default. Якщо знайдено БІЛЬШЕ одного — кидає InvalidOperationException!", "[5].SingleOrDefault()", "5"]
    ]
    add_table_card(s71, 0.8, 1.55, 11.733, 1.85, headers_71, rows_71, [2.0, 3.0, 3.2, 2.5, 1.033], ACCENT_INDIGO)
    add_code_panel(s71, 0.8, 3.55, 11.733, 2.25, [
        'IList<int> singleItem = new List<int>() { 42 };',
        'IList<int> multiples  = new List<int>() { 10, 20, 30 };',
        'int exactlyOne = singleItem.Single();                             // 42 (успіх: рівно 1 елемент)',
        'int safeEmpty  = new List<int>().SingleOrDefault();               // 0 (безпечно: 0 елементів)',
        '// multiples.Single(); // КРИТИЧНИЙ ВИЙНЯТОК: знайдено більше 1 елемента!',
        'Console.WriteLine($"ExactlyOne: {exactlyOne}, SafeEmpty: {safeEmpty}");',
        '// Output: ExactlyOne: 42, SafeEmpty: 0'
    ], 'SingleVsSingleOrDefault.cs')
    add_bottom_banner(s71, "💡 Критична пастка SingleOrDefault:",
                      "SingleOrDefault повертає default ТІЛЬКИ коли елементів 0. Якщо знайдено 2 або більше — він ВСЕ ОДНО кидає виняток!")
    # ==========================================
    # SLIDE 72: ПРИКЛАД SINGLE ТА SINGLEORDEFAULT
    # ==========================================
    s72 = prs.slides.add_slide(blank_layout)
    add_bg(s72)
    add_header(s72, "12 · Елементні оператори (Element)", "Практичний приклад Single та SingleOrDefault у C#", 72)
    add_theory_card(s72, 0.8, 1.55, 5.72, 4.25,
        'Жорсткий контроль унікальності бізнес-даних', ACCENT_INDIGO,
        [
            ('Колекція з 1 елементом:', 'oneElementList.Single() успішно повертає число 7.'),
            ('Колекція з багатьма елементами:', 'intList.Single() кидає виняток, оскільки містить більше одного числа!'),
            ('Фільтр з 1 результатом:', 'intList.Single(i => i < 10) успішно повертає 7 (єдине число менше 10).'),
            ('Порожній список:', 'emptyList.SingleOrDefault() повертає 0 без помилок.')
        ],
        '⚡ Коли використовувати Single:',
        'Виключно тоді, коли поява другого дубліката є критичною аварією цілісності бази даних (наприклад, пошук за унікальним GUID/User ID).'
    )
    add_code_panel(s72, 6.81, 1.55, 5.72, 4.25, [
        'IList<int> oneElementList = new List<int>() { 7 };',
        'IList<int> intList = new List<int>() { 7, 10, 21, 30, 45, 50, 87 };',
        'IList<int> emptyList = new List<int>();',
        '',
        '// 1. Успішні виклики унікальних значень:',
        'int singleVal = oneElementList.Single(); // 7',
        'int emptyVal  = emptyList.SingleOrDefault(); // 0',
        'int singleFiltered = intList.Single(i => i < 10); // 7',
        '',
        'Console.WriteLine($"Single: {singleVal}, Default: {emptyVal}, Filtered: {singleFiltered}");',
        '',
        '// 2. Винятки через дублікати (знайдено більше 1):',
        '// intList.Single(); // InvalidOperationException!',
        '// intList.SingleOrDefault(); // InvalidOperationException!',
        '// emptyList.Single(); // InvalidOperationException!'
    ], 'Program.cs')
    add_bottom_banner(s72, "💡 Продуктивність у БД:",
                      "SingleOrDefault змушує EF Core виконувати SELECT TOP(2), щоб перевірити відсутність дубліката, тоді як FirstOrDefault робить TOP(1).")

    print("Part 2 built successfully (36 slides: 37 - 72).")
