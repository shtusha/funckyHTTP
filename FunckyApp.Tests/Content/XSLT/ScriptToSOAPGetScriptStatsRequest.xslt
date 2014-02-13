<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"
    xmlns:fas="http://schemas.datacontract.org/2004/07/FunckyApp.Services"
    xmlns:fam="http://schemas.datacontract.org/2004/07/FunckyApp.Models"
    xmlns:i="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="/">
      <s:Envelope>
        <s:Body>
           <fas:GetScriptStats>
              <fas:request>
                <fam:Script>
                  <xsl:value-of select="//fam:Program" />
                </fam:Script>
              </fas:request>
            </fas:GetScriptStats>
          </s:Body>
      </s:Envelope>
    </xsl:template>
</xsl:stylesheet>


