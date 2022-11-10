/// <reference path="../../lib/jquery/dist/jquery.js" />
$(() => {
    const DID_NOT_ANSWER = 9;
    const geriatricDepressionScoreInput = $('input[name=GDS]');
    const skipInput = $('input[name=NoGDS]');
    if (skipInput.is(':checked')) {
        $('input[type=radio]').prop('disabled', true);
    }
    skipInput.on('change', (event) => {
        let isChecked = $(event.target).is(':checked');
        if (isChecked) {
            $('input[type=radio]').prop('disabled', true);
            geriatricDepressionScoreInput.val(88);
        } else {
            $('input[type=radio]').prop('disabled', false);
            geriatricDepressionScoreInput.val(null);
        }
    });
    $('input[type=radio]').on('change', (event) => {
        geriatricDepressionScoreInput.val(calculateTotalDepressionScore(...getDepressionScoreData()));
    });
    function getDepressionScoreData() {
        let completedRadios = $('input[type=radio]:checked');
        let answeredQuestions = 0;
        let unansweredQuestions = 0;
        let totalScore = 0;
        completedRadios.each((_index, radio) => {
            let answer = parseInt($(radio).val());
            if (answer != DID_NOT_ANSWER) {
                totalScore += answer;
                answeredQuestions += 1;
            } else if (answer == DID_NOT_ANSWER) {
                unansweredQuestions += 1;
            }
        });
        return [totalScore, answeredQuestions, unansweredQuestions];
    }
    // Total score of completed items + [(Total score of completed items / #of completed items) * (# of unanswered items)]
    function calculateTotalDepressionScore(totalScore, answeredQuestions, unansweredQuestions) {
        if (answeredQuestions + unansweredQuestions == 0) {
            return 88;
        }
        if (answeredQuestions + unansweredQuestions < 15) {
            return null;
        }
        if (unansweredQuestions > 3) {
            return 88;
        }
        let score = totalScore + ((totalScore / answeredQuestions) * unansweredQuestions);
        let isWholeNumber = Number.isInteger(score);
        if (isWholeNumber) {
            return score;
        }
        return Math.round(score);
    }
});