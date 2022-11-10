/// <reference path="../../lib/jquery/dist/jquery.js" />
$(function () {
    const disableCDR = $(`input[name='StandardGlobalCDR']:unchecked`).prop('disabled', true);
    function calculateGlobalCDR(memory, orientation, judgmentAndProblemSolving, communityAffairs, homeAndHobbies, personalCare) {
        for (var l = [memory, orientation, judgmentAndProblemSolving, communityAffairs, homeAndHobbies, personalCare], s = 0, o = 0, c = 0, m = 0, u = 0, d = 0, k = 0, h = 0, p = 0, v = 0, g = 0, f = 0, E = 1; E < l.length; E++)
            l[E] > memory && c++,
                l[E] === memory && o++,
                l[E] < memory && m++,
                0 === l[E] && u++,
                .5 === l[E] && d++,
                1 === l[E] && k++,
                2 === l[E] && h++,
                3 === l[E] && p++;
        if (o >= 3 ? s = memory : c + m >= 3 && Math.abs(c - m) > 1 && (u >= (v = 0) && 0 !== memory && (s = 0, v = u), d >= v && .5 !== memory && (s = .5, v = d), k >= v && 1 !== memory && (s = 1, v = k), h >= v && 2 !== memory && (s = 2, v = h), p >= v && 3 !== memory && (s = 3, v = p)), (3 === c && 2 === m || 2 === c && 3 === m) && (s = memory), .5 === memory) {
            g = 0;
            for (var N = 0; N < l.length; N++)
                l[N] >= 1 && g++;
            s = g >= 3 ? 1 : .5
        }
        if (0 === memory) {
            f = 0;
            for (var b = 0; b < l.length; b++)
                l[b] >= .5 && f++;
            s = f >= 2 ? .5 : 0
        }
        return 4 !== c && 4 !== m || !(2 === u && 2 === d || 2 === u && 2 === k || 2 === u && 2 === h || 2 === u && 2 === p || 2 === d && 2 === k || 2 === d && 2 === h || 2 === d && 2 === p || 2 === k && 2 === h || 2 === k && 2 === p || 2 === h && 2 === p) || (0 === memory && 2 === d ? s = .5 : 0 !== memory || 2 !== k || 2 !== h && 2 !== p ? 0 === memory && 2 === h && 2 === p ? s = 2 : .5 !== memory || 2 !== k || 2 !== h && 2 !== p ? .5 === memory && 2 === h && 2 === p || 1 === memory && 2 === h && 2 === p ? s = 2 : 1 === memory && 2 === u && 2 === d ? s = .5 : 2 !== memory || 2 !== k || 2 !== d && 2 !== u ? 2 === memory && 2 === k && 2 === d ? s = 1 : 3 !== memory || 2 !== h || 2 !== k && 2 !== d && 2 !== u ? 3 !== memory || 2 !== k || 2 !== d && 2 !== u ? 3 === memory && 2 === d && 2 === u && (s = .5) : s = 1 : s = 2 : s = 1 : s = 1 : s = 1),
            1 !== o && 2 !== o || c <= 2 && m <= 2 && (s = memory),
            memory >= 1 && 0 === s && (s = .5),
            s
    }
    function getRadioDoubleValue(controlName) {
        const value = $(`input[name='${controlName}']:checked`).val() || 0;
        return parseFloat(value);
    };
    $("input[type='radio']").on('change', () => {

        let standardSum = 0;
        let supplementalSum = 0;
        $("input.standardSum[type='radio']:checked").each((index, element) => {
            standardSum += parseFloat($(element).val());
        });
        $("input.supplementalSum[type='radio']:checked").each((index, element) => {
            supplementalSum += parseFloat($(element).val());
        });
        $("input[name='StandardCDRSumOfBoxes']").val(standardSum);
        $("input[name='SupplementalGlobalCDR']").val(standardSum + supplementalSum);
        $("input[name='SupplementalCDRSumOfBoxes']").val(supplementalSum);
        const memory = getRadioDoubleValue('Memory');
        const orientation = getRadioDoubleValue('Orientation');
        const judgmentAndProblemSolving = getRadioDoubleValue('JudgmentAndProblemSolving');
        const communityAffairs = getRadioDoubleValue('CommunityAffairs');
        const homeAndHobbies = getRadioDoubleValue('HomesAndHobbies');
        const personalCare = getRadioDoubleValue('PersonalCare');
        const globalCDR = calculateGlobalCDR(memory,orientation,judgmentAndProblemSolving, communityAffairs, homeAndHobbies, personalCare);
        const calculatedCDROption = $(`input[name='StandardGlobalCDR'][value='${globalCDR}']`);
        calculatedCDROption.prop('disabled', false);
        calculatedCDROption.prop('checked', true);
        $(`input[name='StandardGlobalCDR']:unchecked`).prop('disabled', true);
        
    });
});
