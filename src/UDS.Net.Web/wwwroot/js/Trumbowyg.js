$(() => {
  var trumbowygElements = $('.trumbowyg-editor')

  // place database value into input
  var LoadValues = (element) => {
    elementData = GetDBValue(element)
    $(element).trumbowyg('html', elementData.displayValue)
    $(`#${elementData.rawFieldName}`).val(elementData.rawFieldValue)
  }

  // return input element with all relevant element data from it
  GetDBValue = (inputElement) => {
    var currentElementId = $(inputElement).attr('id');
    var rawFieldName = `raw-${currentElementId.substr(10)}`;
    var rawFieldValue = StripHtml($(`#${rawFieldName}`).val())    
    var displayName = `display-${currentElementId.substr(10)}`;
    var displayValue = $(`#${displayName}`).val()
    var wysiwygValue = $(`#${currentElementId}`).trumbowyg('html');

    var elementData = {
      currentElementId,
      rawFieldName,
      rawFieldValue,
      displayValue,
      displayName,
      wysiwygValue
    }

    return elementData;
  }

  // strip tags and special characters
  var StripHtml = (html) => {
    var strippedCode = (html.replace(/<[^>]*>?/gm, ' ')
      .replaceAll("&nbsp;", " ")
      .replace(/  +/g, ' '))
    
    return strippedCode;
  }

  $('.trumbowyg-editor').on("blur", (element) => {
    var elementData = GetDBValue(element.target)
    var wysiwygValue = elementData.wysiwygValue;
    $(`#${elementData.rawFieldName}`).val(StripHtml(wysiwygValue).trim())
    $(`#${elementData.displayName}`).val(wysiwygValue)
  })
    
  // for each trumbowyg class found, create a wysiwyg
  trumbowygElements.each((element) => {
    $(trumbowygElements[element]).trumbowyg({
      tagsToRemove: ['script', 'link'],
      btns: [
        ['strong', 'em', 'del'],
        ['unorderedList', 'orderedList'],
      ]
    })

    LoadValues($(trumbowygElements)[element]);
  })
})