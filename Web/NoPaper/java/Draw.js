const roundedSize          = 20;        // Значение как поделим размер стекла
const thickness            = 3;         // Толщина в px по умолчанию
const defaultColor         = 'black';
const disabledColor        = 'white'; 
const takeActiveColor      = 'green';   // Цвет: следующий взять
const putActiveColor       = 'blue';    // Цвет: следующий поставить
const putColor             = 'red';     // Цвет: поставить
const highlightColor       = 'yellow';  // Цвет: выделение при наведении
const colorNextTakeElement = 'blue';    // Цвет: следующего элемента который необходимо взять

document.addEventListener("DOMContentLoaded", () =>
{
  const pyramid_draw = document.querySelector('.pyramid-draw');

  const pyramidTake          = document.querySelector('.pyramid-take');
  const pyramidTakeContainer = pyramidTake.querySelector('.pyramid-items'); // контейнер где взять

  const pyramidPut           = document.querySelector('.pyramid-put ');
  const pyramidPutContainer  = pyramidPut.querySelector ('.pyramid-items'); // контейнер куда поставить
  const sideNumTextElements  = document.querySelectorAll('.side-num_text'); // стороны

  const sectorManufactElement = document.getElementById('ddListSector'); // Участок
  const selectedSectorID      = sectorManufactElement.value;

  // Замкнутая функция для создания элемента
  const MakePyramidElement = (num, dataElement, container) =>
  {
    // Родительский элемент
    const parrentElementDiv     = document.createElement('div');
    parrentElementDiv.className = 'row-glass';

    // Текстовый элемент с номером пачки или ячейки
    const TextNumElementSpan       = document.createElement('span');
    TextNumElementSpan.className   = 'glass-order'; 
    TextNumElementSpan.textContent = num;

    parrentElementDiv.appendChild(dataElement);
    parrentElementDiv.appendChild(TextNumElementSpan);
    container.appendChild(parrentElementDiv);
  }

  fetch('workplace.aspx/GetDrawDetails',
         {
           method: 'POST',
           headers:
           {
             'Content-Type': 'application/json'
           },
           body: JSON.stringify({
             idSector: selectedSectorID
           })
         }
       )
  .then(response => 
   {
    console.log(response);
    return response.json();
   })
  .then(data =>
  {
    // Не выводим стороны по умолчанию
    sideNumTextElements.forEach(element =>
    {
      if (element.classList.contains('active'))
        element.classList.remove('active');
    });

    const jsonStr = data.d;

    if (!jsonStr || jsonStr.trim() === "")
    {
      pyramid_draw.classList.add("no-active");
      return;
    }

    const parsedData = JSON.parse(jsonStr); // Десериализуем JSON

    if (parsedData.length === 0)
      return;

    // Участок
    const nType = parsedData[0].nType;

    // Выключаем отображение на сборке
    if (nType == 2)
    {
      pyramid_draw.classList.add("no-active")
      return;
    }

    // Уникальные значения Сторон PiramidSide
    const uniqueSides = [...new Set(parsedData.map(item => item.PiramidSide))];

    if (uniqueSides.length > 0)
    {
      let bGetNextFinishedElement = { bGet: true }; // Искать ли следующий элемент в пирамиде где взять

      // Заполнение контейнеров
      uniqueSides.forEach(side =>
      {
        // Фильтруем данные по стороне
        const sideData = parsedData.filter(item => item.PiramidSide === side);

        // Фильтруем по № Пачки
        const uniquePack = [...new Set(sideData.map(item => item.nPack))];

        // Элементы пачек
        const itemsPackPyramidPut = document.createElement('div');
        itemsPackPyramidPut.classList.add('items-packs');
        const itemsPackPyramidTake = itemsPackPyramidPut.cloneNode(false);

        // Рисуем центральную линию
        if (side == 2)
        {
          const marginElement = document.createElement('div');
          marginElement.className = 'center-line';

          pyramidTakeContainer.appendChild(marginElement);
          pyramidPutContainer.appendChild(marginElement.cloneNode(false));
        }

        uniquePack.forEach(nPack =>
        {
          let packData            = sideData.filter(item => item.nPack === nPack); // Элементы пачки
          const nextFinishedElement = bGetNextFinishedElement.bGet             // Подсвечиваемый элемент где взять куда поставить
                                    ? GetNextElement(packData, true, bGetNextFinishedElement)
                                    : - 1;

          // Элементы пачек
          const packToPyramidPut = document.createElement('div');
          packToPyramidPut.classList.add('nPack');
          packToPyramidPut.classList.add(`nPack${nPack}`);

          // Если первая сторона выведим элементы к концу чтобы были ближе к центру
          if (side === 1)
          {
            packToPyramidPut.classList.add('flex-end');
            packData.reverse();
          }

          const packToPyramidTake = packToPyramidPut.cloneNode(false);

          // На участке резка нет пирамиды откуда брать
          if (nType != 1) {
            packData = packData.sort((a, b) => b.nGlassInPackTake - a.nGlassInPackTake);
            // Заполнение контейнера откуда взять
            packData.forEach((data, i) => {
              const reversedIndex = packData.length - 1 - i; // Индекс в обратном порядке
              const num = packData[reversedIndex].nGlassInPackTake;
              let borderColor = defaultColor;
              let elementColor = disabledColor;

              // Если bFinished тогда покажем что элемент стоит в пирамиде
              if (data.bFinished)
                borderColor = disabledColor;

              if (nextFinishedElement === reversedIndex)
                elementColor = takeActiveColor;

              const dataElement = MakeElement(data, elementColor, borderColor);
              MakePyramidElement(num, dataElement, packToPyramidTake);
            });
          }
          else
          {
            pyramidTake.classList.add("no-active")
          }

          // Заполнение контейнера куда поставить
          packData = packData.sort((a, b) => a.nGlassInPack - b.nGlassInPack);
          packData.reverse().forEach((data, i) => {
            const reversedIndex = packData.length - 1 - i; // Индекс в обратном порядке
            const num           = packData[i].nGlassInPack;
            let   elementColor  = disabledColor;

            if (data.bFinished)
              elementColor = putColor;

            if (nextFinishedElement === i)
              elementColor = putActiveColor;

            const dataElement = MakeElement(data, elementColor, disabledColor);
            MakePyramidElement(num, dataElement, packToPyramidPut, putColor);
          });

          itemsPackPyramidTake.appendChild(packToPyramidTake);
          itemsPackPyramidPut.appendChild(packToPyramidPut);    
        });

        pyramidTakeContainer.appendChild(itemsPackPyramidTake);
        pyramidPutContainer.appendChild(itemsPackPyramidPut);
      });


      if (uniqueSides.length === 1)
      {
        const centerLine = document.createElement('div');
        centerLine.className = 'center-line';
        centerLine.classList.add('_add_margin')

        pyramidTakeContainer.appendChild(centerLine);
        pyramidPutContainer.appendChild(centerLine.cloneNode(false));
      }

      sideNumTextElements.forEach(element => {
        if (!element.classList.contains('active'))
          element.classList.add('active');
      });

      // Добавляем события для подстветки элементов
      OnMouseEvent(); 
    }
  });
});

const MakeElement = (dataElement, color, borderColor) =>
{
  const glassElement            = document.createElement('div');
  glassElement.className        = `glass-element glass-element-${dataElement.ID}`;
  glassElement.style.height     = dataElement.Width / roundedSize + 'px';      // Длина делим на константу
  glassElement.style.width      = `${thickness}px`;                            // Толщина

  // цвет элемента
  glassElement.style.background = color;
  glassElement.style.border     = `1px solid ${borderColor}`;

  return glassElement;
}

const OnMouseEvent = () =>
{
  // Обработчики событий для строк таблицы
  document.querySelectorAll('#GridOper tr[data-id]').forEach(row =>
  {
    row.addEventListener('mouseover', () =>
    {
      const id              = row.getAttribute('data-id');
      const relatedElements = document.querySelectorAll(`.glass-element-${id}`);
      relatedElements.forEach(element =>
      {
        element.classList.add('selected'); // Цвет подсветки

        const orderElement = element.nextElementSibling; // Находим соседний элемент
        if (orderElement && orderElement.classList.contains('glass-order'))
          orderElement.classList.add('selected'); // Добавляем класс .selected к glass-order  
      });
    });

    row.addEventListener('mouseout', () =>
    {
      const id              = row.getAttribute('data-id');
      const relatedElements = document.querySelectorAll(`.glass-element-${id}`);
      relatedElements.forEach(element =>
      {
        element.classList.remove('selected'); // Цвет подсветки

        const orderElement = element.nextElementSibling; // Находим соседний элемент
        if (orderElement && orderElement.classList.contains('glass-order'))
          orderElement.classList.remove('selected'); // Добавляем класс .selected к glass-order
      });
    });
  });
}

const GetNextElement = (packData, bReverse = false, bGetNextElement = { bGet: true }) =>
{
  const data = bReverse ? [...packData].reverse() : packData;

  // Находим все элементы с bFinished == true
  const bFinishedElements = data.filter(item => item.bFinished === true);

  // Если все готовы
  if (bFinishedElements.length == packData.length)
    return -1;

  bGetNextElement.bGet = false; // Флаг чтобы не вызвать функцию

  if (bFinishedElements.length > 0)
  {
    const lastFinishedElement = bFinishedElements[bFinishedElements.length - 1]; // Находим последний элемент с bFinished == true
    const lastIndex           = packData.indexOf(lastFinishedElement);           // Находим индекс этого элемента в исходном массиве

    if (bReverse)
    {
      const reverseLastIndex = packData.length - 1 - lastIndex;
      if (reverseLastIndex > 0)
        return reverseLastIndex - 1;
    }
    else
    {
      if (lastFinishedElement + 1 < data.length)
        return lastFinishedElement + 1;
    }
  }

  // Если нет элементов с bFinished == true или нет следующего элемента, возвращаем первый элемент
  return bReverse ? packData.length - 1 : 0;
}