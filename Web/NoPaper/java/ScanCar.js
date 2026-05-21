const BarcodeInput = async (event) => {
  try
  {
    if (event)
      event.preventDefault(); // Останавливаем стандартное поведение формы

    // Получаем значение из текстового поля BarCode
    const barCodeElement = document.getElementById('BarCode');
    const barCodeText = barCodeElement?.value || '';

    // Если штрихкод пуст
    if (!barCodeText) return;

    // Получаем выбранное значение из выпадающего списка Operator
    const operatorDropdown = document.getElementById('Operator');
    const selectedOperator = operatorDropdown
                           ? parseInt(operatorDropdown.options[operatorDropdown.selectedIndex].value, 10) || 0
                           : 0;

    const response = await fetch('ScanCar.aspx/PostBarCode', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        barcode: barCodeText,
        idOperator: selectedOperator
      })
    });

    // Ошибка выполнения запроса
    if (!response.ok)
    {
      ShowMessage("Ошибка обработки штрихкода", true);
      throw new Error(`Error: ${response.status} ${response.statusText}`);
    }

    const data    = await response.json();
    const jsonStr = data.d;

    const textInfoElement = document.getElementById('TextInfo');
    if (textInfoElement) // Краткий вывод информации
      textInfoElement.innerText = jsonStr.message;

    // Устанавливаем оператора, если он изменился
    if (selectedOperator !== jsonStr.idOperator && jsonStr.idOperator !== 0)
      operatorDropdown.value = jsonStr.idOperator;

    await fetchScanLogs(); // Отрисуем таблицу

    barCodeElement.value = '';
  }
  catch (error)
  {
    barCodeElement.value = '';
    console.error('Error for post barcode:', error);
  }
};

// Добавим прослушивание события на нажатие кнопки
document.getElementById('ButBegin')?.addEventListener('click', BarcodeInput);

// Событие нажатия на кнопку Enter
document.addEventListener("DOMContentLoaded", () => {
  const barcodeInput = document.getElementById("BarCode");
  const submitButton = document.getElementById("ButBegin");

  // Нажатие Enter в поле ввода вызывает BarcodeInput
  barcodeInput.addEventListener("keydown", (event) => {
    if (event.key === "Enter") {
      event.preventDefault(); // Останавливаем стандартное поведение
      BarcodeInput(event);
    }
  });

  // Клик по кнопке вызывает BarcodeInput
  submitButton.addEventListener("click", BarcodeInput);
  fetchScanLogs();
});

const fetchScanLogs = async () => {
  try
  {
    const response = await fetch('ScanCar.aspx/GetScanLogs', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      }
    });

    // Ошибка запроса
    if (!response.ok)
    {
      ShowMessage("Ошибка при получении отсканированных данных");
      throw new Error(`Error: ${response.status} ${response.statusText}`);
    }

    const data  = await response.json();
    let jsonStr = JSON.parse(data.d); // предполагаем, что данные вложены в свойство d

    // Обновление таблицы на HTML
    const table = document.querySelector('.gridview');
    const tbody = table.querySelector('tbody');
    tbody.innerHTML = ''; // Очистка существующих строк

    const textInfo = document.getElementById('ScanTextValue');
    if (textInfo) textInfo.innerText = jsonStr.length;

    jsonStr.forEach(row => {
      const tr = document.createElement('tr');
      tr.classList.add("grid-row");

      // Создание и заполнение ячеек
      const timeScanCell = document.createElement('td');
      const dateObj = new Date(row.TimeScan);
      const formattedDate = `${dateObj.toLocaleDateString()} ${dateObj.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;

      timeScanCell.textContent = formattedDate;
      tr.appendChild(timeScanCell);

      const scanTextCell = document.createElement('td');
      scanTextCell.textContent = row.ScanText;
      tr.appendChild(scanTextCell);

      const gosNumber = document.createElement('td');
      gosNumber.textContent = row.GosNumber;
      tr.appendChild(gosNumber);

      tbody.appendChild(tr);
    });

  } catch (error)
  {
    console.error('Error fetching scan logs:', error);
  }
};
