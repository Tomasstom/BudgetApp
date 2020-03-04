(function () {
    var requestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    var api = {
        removeExpense: function (expenseId) {
            return $.ajax({
                method: 'DELETE',
                url: '/expenses/' + expenseId,
                headers: {'X-CSRF-TOKEN': requestVerificationToken}
            })
        }
    };
    
    $('tr[data-expense-id]').each(function () {
        var $tr = $(this);
        var expenseId = parseInt($tr.data('expense-id'));
        
        var $deleteExpenseIcon = $tr.find('i[data-delete-expense]');

        $deleteExpenseIcon.click(function () {
            bootbox.confirm({
                message: 'Czy na pewno chcesz usunąć ten wydatek?',
                buttons: {
                    confirm: {
                        label: 'Usuń',
                        className: 'btn-danger'
                    },
                    cancel: {
                        label: 'Anuluj',
                        className: 'btn-default'
                    }
                },
                callback: function (result) {
                    if (result !== true) {
                        return;
                    }
                    api.removeExpense(expenseId)
                        .done(function () {
                            $tr.remove();
                        })
                        .fail(function(xhr, err) {
                            alert(xhr.responseJSON.message);
                        });
                }
            })
        })
    })
})();