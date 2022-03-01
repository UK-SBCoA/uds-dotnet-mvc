$(function () {
    $("input[type='radio']").on('change', () => {

        var standardSum = 0;
        var supplementalSum = 0;
    
        $("input.standardSum[type='radio']:checked").each((index, element) => {
            standardSum += parseFloat($(element).val());
        })
    
        $("input.supplementalSum[type='radio']:checked").each((index, element) => {
            supplementalSum += parseFloat($(element).val());
        })
        

        $("input[name='StandardCDRSumOfBoxes']").val(standardSum)
        $("input[name='SupplementalGlobalCDR']").val(standardSum + supplementalSum)
        $("input[name='SupplementalCDRSumOfBoxes']").val(supplementalSum)
    });
})
