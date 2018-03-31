<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <body>
        <form method="post" action="edittool.asp">
          <h2>Xml editor:</h2>
          <table border="1">
            <tr bgcolor="#9acd32">
              <th>Key</th>
              <th>Translation</th>
            </tr>
            <xsl:for-each select="TransLation/MenuItems/KeyValue">
              <tr>
                <td>
                  <xsl:value-of select="@Key"/>
                </td>
                <td>
                  <input type="text">
                    <xsl:attribute name="id">
                      <xsl:value-of select="@Key"/>
                    </xsl:attribute>
                    <xsl:attribute name="name">
                      <xsl:value-of select="@Key"/>
                    </xsl:attribute>
                    <xsl:attribute name="value">
                      <xsl:value-of select="@Value"/>
                    </xsl:attribute>
                  </input>
                </td>
              </tr>
            </xsl:for-each>
          </table>
          <br/>
          <input type="submit" id="btn_sub" name="btn_sub" value="Save"/>
          <input type="reset" id="btn_reset" name="btn_reset" value="Reset"/>
        </form>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>