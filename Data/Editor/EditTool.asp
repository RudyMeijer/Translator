<%
function loadFile(xmlfile,xslfile)
Dim xmlDoc,xslDoc
'Load XML and XSL file
set xmlDoc = Server.CreateObject("Microsoft.XMLDOM")
xmlDoc.async = false
xmlDoc.load(xmlfile)
set xslDoc = Server.CreateObject("Microsoft.XMLDOM")
xslDoc.async = false
xslDoc.load(xslfile)
'Transform file
Response.Write(xmlDoc.transformNode(xslDoc))
end function

function updateFile(xmlfile)
Dim xmlDoc,rootEl,f
Dim i
'Load XML file
set xmlDoc = Server.CreateObject("Microsoft.XMLDOM")
xmlDoc.async = false
xmlDoc.load(xmlfile)

'Set the rootEl variable equal to the root element
Set rootEl = xmlDoc.documentElement

'Loop through the form collection
for i = 1 To Request.Form.Count
  'Eliminate button elements in the form
  if instr(1,Request.Form.Key(i),"btn_")=0 then
    'The selectSingleNode method queries the XML file for a single node
    'that matches a query. This query requests the value element that is
    'the child of a field element that has an id attribute which matches
    'the current key value in the Form Collection. When there is a match -
    'set the text property equal to the value of the current field in the
    'Form Collection.
    set f = rootEl.selectSingleNode("field[@id='" & _
    Request.Form.Key(i) & "']/value")
    f.Text = Request.Form(i)
  end if
next

'Save the modified XML file
xmlDoc.save xmlfile

'Release all object references
set xmlDoc=nothing
set rootEl=nothing
set f=nothing

'Load the modified XML file with a style sheet that
'allows the client to see the edited information
loadFile xmlfile,server.MapPath("updated.xsl")
end function

'If form is submitted, update the XML file and display result
' - if not, transform the XML file for editing
if Request.Form("btn_sub")="" then
  loadFile "english.xml","edit.xsl"
else
  updateFile "edit.xsl"
end if
%>