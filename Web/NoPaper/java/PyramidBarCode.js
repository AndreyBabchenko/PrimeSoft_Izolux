let   maxPyramidBarCodeLength = window.appConfig.maxPyramidBarCodeLength;
const barCodeInputs_rowIndex = []; // Массив
function loadBarCodeInputs()
{
  const barCodeInputs = document.querySelectorAll('.pyramid-barcode__input');
  barCodeInputs.forEach(barCodeInput => {
    const rowIndex = barCodeInput.getAttribute('data-rowindex');
    barCodeInputs_rowIndex.push(rowIndex);
  });
}


const focusGlassTextBox = (text = "") => {
  const barCodeTxtInput = document.getElementById('BarCodeTxtInput');
  barCodeTxtInput.value = text;
  barCodeTxtInput.focus();

  const activeTextBox = sessionStorage.getItem('activeTextBox');
  if (!activeTextBox)
    return;

  loadBarCodeInputs();

  const { rowIndex } = JSON.parse(activeTextBox);
  const currentIndex = barCodeInputs_rowIndex.indexOf(rowIndex);

  if (currentIndex !== -1)
  {
    const curTextBoxID = `GridOper_CurrentPyramidBarCode_${barCodeInputs_rowIndex[currentIndex]}`;
    const nextTextBox = document.getElementById(curTextBoxID);
    nextTextBox.value = "";
  }
}

const focusNextTextBox = () => {
  if (!barCodeInputs_rowIndex.length > 0)
    loadBarCodeInputs();

  const activeTextBox = sessionStorage.getItem('activeTextBox');
  if (!activeTextBox)
    return;

  const { rowIndex } = JSON.parse(activeTextBox);
  const currentIndex = barCodeInputs_rowIndex.indexOf(rowIndex);

  if (currentIndex === -1)
    return;

  // Последнее поле для ввода ставим фокус на шапку
  if (currentIndex === barCodeInputs_rowIndex.length - 1)
  {
    const headerTextBox = document.getElementById('BarCodeTxtInput');

    window.scrollTo({
      top: 0,
      behavior: 'smooth' // плавный скролл
    });

    if (headerTextBox)
      headerTextBox.focus();
  }
  else // Иначе переходим к следующему элементу
  {
    const nextTextBoxID = `GridOper_CurrentPyramidBarCode_${barCodeInputs_rowIndex[currentIndex + 1]}`;
    const nextTextBox   = document.getElementById(nextTextBoxID);
    if ( nextTextBox )
      nextTextBox.focus();
  }
}

const saveActiveTextBox = (rowIndex) =>
{
  sessionStorage.setItem("activeTextBox", JSON.stringify({ rowIndex }));
}

const clearActiveTextBox = () =>
{
  sessionStorage.removeItem("activeTextBox");
  return true;
}

function translateRussianToEnglish(text)
{
  const rusToEngMap = {
    'а': 'f', 'б': ',', 'в': 'd', 'г': 'u', 'д': 'l', 'е': 't', 'ё': '`', 'ж': ';',
    'з': 'p', 'и': 'b', 'й': 'q', 'к': 'r', 'л': 'k', 'м': 'v', 'н': 'y', 'о': 'j',
    'п': 'g', 'р': 'h', 'с': 'c', 'т': 'n', 'у': 'e', 'ф': 'a', 'х': '[', 'ц': 'w',
    'ч': 'x', 'ш': 'i', 'щ': 'o', 'ъ': ']', 'ы': 's', 'ь': 'm', 'э': '\'', 'ю': '.',
    'я': 'z', 'А': 'F', 'Б': '<', 'В': 'D', 'Г': 'U', 'Д': 'L', 'Е': 'T', 'Ё': '~',
    'Ж': ':', 'З': 'P', 'И': 'B', 'Й': 'Q', 'К': 'R', 'Л': 'K', 'М': 'V', 'Н': 'Y',
    'О': 'J', 'П': 'G', 'Р': 'H', 'С': 'C', 'Т': 'N', 'У': 'E', 'Ф': 'A', 'Х': '{',
    'Ц': 'W', 'Ч': 'X', 'Ш': 'I', 'Щ': 'O', 'Ъ': '}', 'Ы': 'S', 'Ь': 'M', 'Э': '"',
    'Ю': '>', 'Я': 'Z'
  };

  return text.split('').map(char => rusToEngMap[char] || char).join('');
}

// Сканирование штрихкод пирамиды в таблице. (Пример BS001)
async function handlePyramidBarCodeFetch(e, textBox)
{
  e.preventDefault();

  const pyramidBarCode            = translateRussianToEnglish(textBox.value);  // введенное в textBox pначение
  const rowIndex                  = textBox.getAttribute("data-rowindex");     // Атрибут индекс строки, для последующего перехода на новый текстбокс
  const idBarCode                 = Number(textBox.getAttribute('data-idbarcode'));         // аттрибут поля хранящий idBarCodeID
  const idPyramid                 = Number(textBox.getAttribute('data-idPyramid')) || 0;    // аттрибут поля хранящий idPiramid
  const idSectorManufact          = textBox.getAttribute('data-idSector');     // аттрибут поля хранящий idSector
  const sIdGlassProcessingPyramid = textBox.getAttribute('data-sidglass');     // аттрибут поля хранящий idGlassProcessingPyramid

  if (!maxPyramidBarCodeLength || isNaN(maxPyramidBarCodeLength)) 
    maxPyramidBarCodeLength = window.appConfig.maxPyramidBarCodeLength;

  // Минимальное количество символов
  if (pyramidBarCode.length !== maxPyramidBarCodeLength)
    return;

  try
  {
    // Запрос на сохранение введенного значения
    const response = await fetch('workplace.aspx/WritePyramidBarCodeByTextBox', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(
      {
        idBarCode,
        idPyramid,
        idSectorManufact,
        pyramidBarCode,
        sIdGlassProcessingPyramid,
        isCheck: false
      })
    });

    if (!response.ok)
    {
      ShowMessage("Ошибка обработки штрихкода", false);
      throw new Error(`Error: ${response.status} ${response.statusText}`);
    }

    // Если успех сохраним текущий текстбокс как активный для перехода на следующий
    saveActiveTextBox(rowIndex);

    const data    = await response.json();
    const jsonStr = data.d;

    if (!jsonStr.isSuccess)
      throw new Error(jsonStr.message || "Ошибка обработки");

    // Фокусируемся на следующий текстбокс в таблице
    focusNextTextBox();
  }
  catch (ex)
  {
    textBox.value = "";
    console.error("Error for post barcode:", error);
    ShowMessage(ex.message, false);
  }
  
}

// Переменная для блокировки повтрного ввода (при сканировании через сканер, происходит вызов функции два раза, от чего всегда выходит сообщение что штрихкод отсканирован)
let isBarcodeProcessing = false;

async function handleBarCodeInput(e, textBox) {
  // При блокировке
  if (isBarcodeProcessing)
  {
    e.preventDefault(); // Блокируем действия по умолчанию

    textBox.value = ""; // стираем что было введено
    textBox.focus();    // ставим фокус
    return;
  }

  const ddlSector        = document.getElementById('ddListSector'); // Участок
  const idSectorManufact = ddlSector.value; // Текущий выбранный участок

  const ddlPerson        = document.getElementById('ddListPerson'); // Оператор
  const idOperator       = ddlPerson.value;                         // Текущий оператор выбранный в интерфейсе {idSheduleOperator}

  const selectedOption    = ddlPerson.options[ddlPerson.selectedIndex];
  const idSheduleOperator = selectedOption.getAttribute('idSheduleOperator');

  const barcode = translateRussianToEnglish(textBox.value).toLowerCase();
  const key = e.which || e.keyCode;
  const maxCharacters = 12; // Максималь  ное количество символов на примере : "BS0000688458"

  // Нажали Enter или ввели максимально допустимое количество символов
  if (key === 13 || barcode.length >= maxCharacters)
  {
    e.preventDefault();
    
    // Выставляем флаг блокировки
    isBarcodeProcessing = true;
    setTimeout(() => isBarcodeProcessing = false, 1000); // Выставляем таймер для разблокировки через 1 секунду

    const response = await fetch('workplace.aspx/WriteBarCode', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(
      {
        barcode,
        idSectorManufact,
        idOperator,
        idSheduleOperator
      })
    });

    if (!response.ok)
    {
      ShowMessage("Ошибка обработки штрихкода", false);
      throw new Error(`Error: ${response.status} ${response.statusText}`);
    }

    const data = await response.json();
    const jsonStr = data.d;

    const result = jsonStr.Data; // Вернувшиеся данные запроса

    // Не известный штрихкод
    if (result.Type === 0)
      ShowMessage(jsonStr.Message, false);
    // Введен штрихкод оператора
    else if (result.Type === 1) {
      ddlPerson.value = result.ID.toString(); // Установить выбранный элемент

      const selectedOption = ddlPerson.options[ddlPerson.selectedIndex];
      if ( selectedOption ) 
        selectedOption.setAttribute("idSheduleOperator", result.idSheduleOperator);

      // Фильтрация списка участков
      const sectorOptions = Array.from(ddlSector.options);
      const idSector = result.IDSectorManufact;

      if (idSector > 0) {
        // Оставляем только нужный участок
        sectorOptions.forEach(option => {
          option.hidden = option.value !== idSector.toString();
        });

        ddlSector.value = idSector.toString();
      }
      else {
        // Показываем все варианты участков
        sectorOptions.forEach(option => {
          option.hidden = false;
        });
      }

      ShowMessage(jsonStr.Message, jsonStr.IsSuccess);
    }
    // Штрихкод внутрицеховой пирамиды
    else if (result.Type === 2)
      ShowMessage(jsonStr.Message, jsonStr.IsSuccess);
    else if (result.Type === 3)
    {
      ShowMessage(jsonStr.Message, jsonStr.IsSuccess);

      // Штрихкод не удачный и требует повторного сканирования?
      if (!jsonStr.IsSuccess && IsNeedRetryScan)
      {

      }
    }

    textBox.value = "";
  }
}

/* Возможно понадобиться, но пока комментирую из-за Postback во время ввода внутрицехового штрихкода
при загрузке всех компонентов выставим фокус на поле для ввода штрихкода
window.addEventListener('DOMContentLoaded', function () {
  const input = document.getElementById('BarCodeTxtInput');
  if (input) {
    input.focus();
  }
});
*/

function handleBarCodeInputold(e, textBox)
{
  e.preventDefault();

  barcode = translateRussianToEnglish(textBox.value);  // Переводим текущий текст

  const key = e.which || e.keyCode;
  const currentKey = String.fromCharCode(key);

  if (key === 13 || currentText.length >= maxCharacters)
  {
    // Обработка ввода штрих кода пирамиды 
    if ( currentText.length === pyramidBarCodeCountLetter )
    {
      const barCodePrefix = currentText.substring(0, pyramidPrefixBarCode.length).toLowerCase();

      if (barCodePrefix === pyramidPrefixBarCode)
      {
        __doPostBack("CurrentPyramidBarCode", `OnWriteBarCodePyramid`)
        return false;
      }
    }
    else if (currentText.length === operBarCodeCountLetter) // Обработка ввода штрих-кода оператора
    {
      const barCodePrefix = currentText.substring(0, operPrefixBarCode.length).toLowerCase();

      if (barCodePrefix === operPrefixBarCode)
        {
          __doPostBack("CurrentOperBarCode", `OnWriteOperBarCodeOperator`);
          return false;
        }
    }

    if (isHeader) // Если вводим в шапке то помечаем деталь как готовую
      __doPostBack("CurrentPyramidBarCode", "OnWriteBarCodeGlassByTextBox")
  }

}

// Сохраним оператора в localStorage
function saveCurrentOperator(t)
{
  console.log(`оператор: ${t.value}`);
  localStorage.setItem("selectedPerson", t.value);

  //const operatorDropdown = document.getElementById("Operator");
  //if (operatorDropdown) 
  //  operatorDropdown.value = t.value;
}
