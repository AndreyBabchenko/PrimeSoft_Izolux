window.addEventListener('DOMContentLoaded', () => {
  const savedArrow = sessionStorage.getItem('sort-arrow');

  if (savedArrow !== null)
  {
    const savedArrowArgs = savedArrow.split('-');
    const classesToAdd = savedArrowArgs[1].split(' ');

    const element = document.getElementById(`${savedArrowArgs[0]}`);
    if (element)
      element.classList.add(...classesToAdd);
    return;
  }

  const arrowElement = document.querySelector('.arrow');

  if (arrowElement)
  {
    document.querySelector('.arrow').classList.add('_show');
    sessionStorage.setItem('sort-arrow', 'NameSawTask-_show');
  }
}); 

const showHideSortArrow = (currentNode) => {
  const currentArrow = currentNode.querySelector('.arrow');
  if (currentArrow.classList.contains('_show'))
  {
    currentArrow.classList.toggle('open');
    const classList = currentArrow.classList.value.replace('arrow', '').trim();
    const saveArrow = `${currentArrow.id}-${classList}`
    sessionStorage.setItem('sort-arrow', `${saveArrow}`)
  }
  else
  {
    const arrows = document.querySelectorAll('.arrow');
    arrows.forEach( (arrow) => {
      arrow.classList.remove('_show');
    });

    currentArrow.classList.add('_show');
    sessionStorage.setItem('sort-arrow', `${currentArrow.id}-_show`)
  }
}
