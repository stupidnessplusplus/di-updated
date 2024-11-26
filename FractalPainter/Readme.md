﻿# Задание FractalPainter

1. Разминка. В классе Program сделайте так, чтобы App
   создавался контейнером. Удалите у App конструктор без параметров
   и сделайте так, чтобы контейнер инжектировал в App список IApiAction.

2. INeed<T>. Изучите код KochFractalAction.
   Изучите механику работы INeed<T> и DependencyInjector.
   Оцените такой подход к управлению зависимостями.

3. Рефакторинг. Измените класс KochFractalAction так,
   чтобы его зависимости IImageSettingsProvider и Palette инжектировались
   явно через конструктор, без использования интерфейса INeed.

Подсказка. Сложность в том, чтобы в App и KochFractalAction
оказались ссылки на один и тот же объект AppSettings.

Убедитесь, что настройка палитры для рисования кривой Коха всё ещё работает.

4. Еще рефакторинг. Изучите KochFractalAction и поймите, что
   на самом деле IImageSettingsProvider и Palette ему не нужны. Измените его так,
   чтобы он принимал только KochPainter.

5. Фабрика. Аналогично удалите INeed,
   и явное использование контейнера из класса DragonFractalAction.
   Дополнительное ограничение — нельзя менять публичный интерфейс DragonPainter.
   Особенность в том, что одна из зависимостей DragonPainter —
   DragonSettings оказывается известной только в процессе работы экшена.
   Из-за этого вы не можете просить инжектировать в конструктор уже готовый Painter.
   Вместо этого инжектируйте фабрику DragonPainter-ов. В проекте уже есть интерфейс IDragonPainterFactory. 
   Создайте класс, реализующий этот интерфейс и проверьте, что изменение настроек работает

6. Новая зависимость. Переведите DragonPainter на использование цветов палитры,
   как это сделано в KochPainter.

Убедитесь, что экшен настройки палитры работает как надо.
Если вы всё сделали правильно, то для добавления зависимости вам не пришлось
править код работы с контейнером вообще. Магия!

7. Избавьтесь от остальных использований INeed и удалите этот интерфейс
   и класс DependencyInjector из проекта.