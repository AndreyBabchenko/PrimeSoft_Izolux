window.onload = function () {
  const operatorContainer = document.querySelector('.operator');

  if (operatorContainer) {
    const operatorName = sessionStorage.getItem('operName'); // Извлекаем из sessionStorage

    if (operatorName) {
      operatorContainer.innerText = operatorName; // Устанавливаем текст в элемент
    }
  }
};

const BarcodeInput = async () =>
{
  const pyramidPrefixBarCode = 'pp';    // Префикс на пирамиде
  const pyramidBarCodeCountLetter = 5;  // Лимит символов для ввода шк пирамиды   

  const operPrefixBarCode = 'oper';     // Префикс оператора
  const operBarCodeCountLetter = 6;     // Лимт символов для ввода шк оператора

  const barCodeElement = document.getElementById('BarCode');
  const currentBarCode = barCodeElement ? barCodeElement.value : '';      // Введенный шк

  const sectorDropdown = document.getElementById('ddSector');
  const selectedSector = sectorDropdown
                         ? parseInt(sectorDropdown.options[sectorDropdown.selectedIndex].value, 10) || 0
                         : 0;

  let idOperator = parseInt(sessionStorage.getItem('idOperator'), 10) || 0; // Текущий оператор
  let idPyramid  = parseInt(sessionStorage.getItem('idPyramid'), 10) || 0; // Текущий оператор

  barCodeElement.value = '';

  // Обработка ввода штрих кода пирамиды 
  if (currentBarCode.length === pyramidBarCodeCountLetter)
  {
    const barCodePrefix = currentBarCode.substring(0, pyramidPrefixBarCode.length).toLowerCase();

    if (barCodePrefix === pyramidPrefixBarCode)
    {
      const response = await fetch('ScanBarcodes.aspx/PostPyramidBarCode',
                             {
                               method: 'POST',
                               headers: {
                                 'Content-Type': 'application/json'
                               },
                               body: JSON.stringify({
                                 pyramidBarCode:  currentBarCode
                               })
                             });

      const result = await response.json();
      const data = result.d;

      ShowMessage(data.message, data.isSuccess);
      sessionStorage.setItem('idPyramid', data.idPyramidBarCode);

      return false;
    }
  }
  else if (currentBarCode.length === operBarCodeCountLetter) // Обработка ввода штрих-кода оператора
  {
    const barCodePrefix = currentBarCode.substring(0, operPrefixBarCode.length).toLowerCase();

    if (barCodePrefix === operPrefixBarCode)
    {
      const response = await fetch('ScanBarcodes.aspx/PostOperatorBarCode',
                             {
                               method: 'POST',
                               headers: {
                                 'Content-Type': 'application/json'
                               },
                               body: JSON.stringify({
                                 operBarCode:  currentBarCode
                               })
                             });

      const result = await response.json();
      const data   = result.d;

      ShowMessage(data.message, data.isSuccess);
      sessionStorage.setItem('idOperator', data.idOperator);
      sessionStorage.setItem('operName',   data.operName);

      const operatorName = document.querySelector('.operator');

      if (operatorName)
        operatorName.innerText = data.operName;

      return false;
    }
  }

  // Если ввели штрихкод и отсканировали оператора
  if (!currentBarCode || idOperator === 0)
  {
    const response = await fetch('ScanBarcodes.aspx/PostMessage',
                           {
                             method: 'POST',
                             headers: {
                               'Content-Type': 'application/json'
                             },
                             body: JSON.stringify({
                               currentBarCode,
                               idOperator
                             })
                           });

    const result = await response.json();
    const data = result.d;

    if ( data.isShow )
      ShowMessage(data.message, data.isSuccess);
    return false;
  }

  // Иначе обрабатываем ШК стекла или СП
  const response = await fetch('ScanBarcodes.aspx/PostBarCode',
                         {
                           method: 'POST',
                           headers: {
                             'Content-Type': 'application/json'
                           },
                           body: JSON.stringify({
                             barCodeGlassText    : currentBarCode,
                             idPyramidScanBarCode: idPyramid,
                             idOperator          : idOperator,
                             idSector            : selectedSector
                           })
                         });

  const result = await response.json();
  const data = result.d;

  ShowMessage(data.message, data.isSuccess);
  return false;
}