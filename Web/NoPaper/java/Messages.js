function ShowMessage(message, isSuccess = false)
{
  let messageBlock = document.querySelector('.message');

  // Добавление текста сообщения
  messageBlock.textContent = message;

  // Добавление класса 'show' для отображения блока сообщения
  messageBlock.classList.add('show');

  // Очистка всех предыдущих классов состояния (success, error)
  if (messageBlock.classList.contains('success'))
    messageBlock.classList.remove('success');

  if (messageBlock.classList.contains('error'))
    messageBlock.classList.remove('error');

  if (isSuccess) 
    messageBlock.classList.add('success');
  else 
    messageBlock.classList.add('error');

  // Установка таймера для скрытия блока сообщения через 3 секунды
  setTimeout(function () {
    // Удаление класса 'show' для скрытия блока сообщения
    messageBlock.classList.remove('show');
  }, 4000);
}