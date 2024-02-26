// JavaScript source code
$(function () {

    // The default header breaking function is breaking on camel casing which
    // screws up our longer namespace representations.
    // Update to add break after keyword only. 
    function breakPlainText(text) {
        if (!text) {
            return text;
        }

        return text.replace(/(Namespace|Class|Enum|Struct|Type|Interface)/g, '$1<wbr>');
    }

    $("h1.text-break").each(function () {
        const $this = $(this);

        $this.html(breakPlainText($this.text()));
    });

    $(".table tr td:first-child *").each(function () {
        const $this = $(this);

        $this.html(breakPlainText($this.text()));
    });

    // Fix the width of the right sidebar so we don't lose content.
    const scrollbarWidth = 3.5 * (window.innerWidth - document.body.offsetWidth);
    $(".sideaffix").each(function () {
        const $this = $(this);

        $this.width($this.parent().outerWidth() + scrollbarWidth);
    });
});