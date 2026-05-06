async function buscarCep(cep: string) {
    const cepLimpo = cep.replace(/\D/g, '');
    if (cepLimpo.length !== 8) return null;

    try {
        const res = await fetch(`https://viacep.com.br/ws/${cepLimpo}/json/`);
        const data = await res.json();
        return data.erro ? null : data;
    } catch {
        return null;
    }
}

$(function () {

    $('#Cep').on('blur', async function () {
        const cep = $(this).val() as string;
        const endereco = await buscarCep(cep);
        if (!endereco) return;

        $('#Logradouro').val(endereco.logradouro);
        $('#Bairro').val(endereco.bairro);
        $('#Cidade').val(endereco.localidade);
        $('#Estado').val(endereco.uf);
    });

    $('[data-action-url]').on('click', function () {
        const btn = $(this);
        const url: string = btn.data('action-url');
        const msg: string = btn.data('confirm') || '';

        if (msg && !confirm(msg)) return;

        btn.prop('disabled', true);

        $.ajax({
            url: url,
            method: 'PUT',
            success: function () {
                window.location.reload();
            },
            error: function (xhr: JQuery.jqXHR) {
                const resposta = xhr.responseJSON;
                alert(resposta?.mensagem || 'Erro ao processar a solicitação.');
                btn.prop('disabled', false);
            }
        });
    });

});
