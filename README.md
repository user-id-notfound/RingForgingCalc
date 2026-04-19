# RingForgingCalc

Веб-приложение для автоматизированного расчета параметров поковок «кольцо раскатное» с визуализацией технических чертежей. Выполняет расчеты размеров заготовки, допусков и массы на основе математической модели.

## ✨ Возможности

- Интерактивная форма ввода параметров поковки
- Автоматический расчет размеров заготовки с учетом допусков по ГОСТ
- Расчет массы поковки (номинальной и максимальной)
- Два варианта расчета: с пробой и без пробы
- SVG-визуализация технических чертежей

## 🛠 Технологии

| Технология | Версия | Назначение |
|------------|--------|------------|
| **ASP.NET Core** | 8.0 | Backend фреймворк |
| **C#** | 12 | Язык программирования |
| **Bootstrap** | 5 | UI фреймворк |
| **SVG** | - | Визуализация чертежей |
| **jQuery Validation** | - | Клиентская валидация |

## 📁 Структура проекта

RingForgingCalc/
├── Controllers/
│   ├── HomeController.cs
│   └── CalculationController.cs
├── Models/
│   ├── CalculationInputModel.cs
│   ├── CalculationResult.cs
│   └── ViewModels/
│       └── ResultViewModel.cs
├── Services/
│   ├── ICalculationService.cs
│   ├── CalculationService.cs
│   ├── IDrawingService.cs
│   └── DrawingService.cs
├── Views/
│   ├── Home/
│   │   └── Index.cshtml
│   ├── Calculation/
│   │   └── Result.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       └── _ValidationScriptsPartial.cshtml
├── wwwroot/
│   ├── css/
│   │   └── site.css
│   └── lib/
├── appsettings.json
├── Program.cs
├── RingForgingCalc.csproj
└── README.md

## Использование

Пример работы

1. Откройте главную страницу приложения
2. Заполните форму параметрами:
  D — наружный диаметр детали (мм)
  d — внутренний диаметр детали (мм)
  H — высота детали (мм)
  X, Y, Z — параметры раскроя
  Q — напуск на пробу (мм)
3. Нажмите кнопку «Рассчитать»
4. Просмотрите результаты:
  Размеры заготовки с допусками
  Расчет массы (номинальная и максимальная)
  SVG-чертежи для вариантов с пробой и без пробы

Скриншоты: 

Форма ввода данных: <img width="1050" height="601" alt="image" src="https://github.com/user-attachments/assets/36c0bb00-73aa-41c8-8598-6c62b93e900e" />

Результаты расчета: <img width="1053" height="1198" alt="image" src="https://github.com/user-attachments/assets/85fa3b90-05f1-4d9b-b403-86622cf8f46f" />

Отображение результатов на чертежах: <img width="1072" height="885" alt="image" src="https://github.com/user-attachments/assets/b9ce87b1-6c4d-4354-a0d6-ae6d1266412d" />


