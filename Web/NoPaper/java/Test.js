const fetchTaskId = async (event) => {
  try {
    event.preventDefault();
    const response = await fetch('scan1.aspx/GetTaskId', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({})
    });

    // Проверка на успешность ответа
    if (!response.ok) {
      throw new Error(`Ошибка: ${response.status} ${response.statusText}`);
    }

    const data = await response.json(); // Берем ответ как текст
    console.log(data); // Логируем строку
    ShowMessage(data, true); // Отправляем строку в ShowMessage

  } catch (error) {
    console.error("Ошибка запроса:", error);
    ShowMessage("Ошибка запроса", false);
  }
};

document.getElementById("ClientButton").addEventListener("click", fetchTaskId);
