<html>
    <head>
        <title>Search Test Page</title>
        <meta name="robots" content="index,follow" />
    </head>
    <body>
        <p>
            <a href="Search.aspx">Search Page</a>
        </p>
        <a href="content/plaintext.txt">text</a>
        <!--SEARCHAROONOINDEX-->This text won't be indexed (but the links will be followed)
        <a href="#" onclick="window.location='content/Kilimanjaro.pdf'">Kilimanjaro</a>
        <a href="content/Decorator.ppt">decorator</a>
        <!--/SEARCHAROONOINDEX-->
        <a href="content/Marathoning.doc">marathon</a>
    </body>
</html>