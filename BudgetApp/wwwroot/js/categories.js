(function () {
    var requestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    var api = {
        removeCategory: function (categoryId) {
            return $.ajax({
                method: 'DELETE',
                url: '/categories/' + categoryId,
                headers: {'X-CSRF-TOKEN': requestVerificationToken}
            })
        }
    };

    $('li[data-category-id]').each(function () {
        var $li = $(this);
        var categoryId = parseInt($li.data('category-id'));

        var $deleteCategoryIcon = $li.find('i[data-delete-category]');

        $deleteCategoryIcon.click(function () {
            bootbox.confirm({
                message: 'Czy na pewno chcesz usunąć tę kategorię?',
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
                    api.removeCategory(categoryId)
                        .done(function () {
                            $li.remove();
                        })
                        .fail(function(xhr, err) {
                            alert(xhr.responseJSON.message);
                        });
                }
            })
        })
    })
})();