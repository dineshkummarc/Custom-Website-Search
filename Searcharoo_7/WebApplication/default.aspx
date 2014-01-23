<html>
    <head>
        <title>Search Test Page</title>
        <meta name="robots" content="index,follow" />
    </head>
    <body>
    
        <p>
            <a href="Search.aspx">Search Page</a>
        </p>
        <p>
        <a href="content/test">default page test 2</a>
        <a href="content/test/">default page test 1</a>
        <a href="content/test/default.aspx">default page test 1</a>
        
         <!--SEARCHAROONOINDEX-->
        <a href="content/OpenXMLsample.xlsx">OpenXml</a>
        <a href="content/OrcasData.pptx">Orcas</a>
        <a href="content/msbuild.docx">MSBUild</a>
        <a href="content/plaintext.txt">text</a>
        <a href="content/bed.jpg">image</a>
        <a href="content/tiger.jpg">tiger image</a>
        
        <a href="#" onclick="window.location='content/Kilimanjaro.pdf'">Kilimanjaro</a>
        
       This text won't be indexed (but the links will be followed)
        <a href="content/Decorator.ppt">decorator</a><!--/SEARCHAROONOINDEX-->
        
        <!--SEARCHAROONOFOLLOW--> 
        <a href="#" onclick="window.location='content/Kilimanjaro.pdf'">Kilimanjaro</a>
        <!--/SEARCHAROONOFOLLOW-->
        <a href="content/Marathoning.doc">marathon</a>
        <a href="content/za.html">zambia</a>
        <a href="content/wz.html">Swaziland</a>
        <a href="content/uk.html">UK</a>
        </p>
    </body>
</html>