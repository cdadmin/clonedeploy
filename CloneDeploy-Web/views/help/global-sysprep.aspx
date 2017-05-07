<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#global').addClass("nav-current");
            $('#global-sysprep').addClass("nav-current-sub");
        });
    </script>
    <h1>Global->Sysprep</h1>
    <p>
        Sysprep tags allow you make changes on the fly to a sysprep file during the imaging process. You can use this to
        replace almost any existing value in the sysprep answer file, you cannot add new entries. To use this put in the
        opening sysprep tag and the closing sysprep tag and the contents of what you want in between them.
        Example: Opening Tag: &lt;ComputerName&gt; Closing Tag &lt;/ComputerName&gt;. Once defined the tags can then be assigned in the image profile.
        The contents can contain anything you want including variables such as custom attributes. The contents can also contain other
        nested xml tags. The formatting is slightly different depending on which imaging environment you are using.
    </p>

    <h3>Linux Imaging Environment Examples</h3>

    <p>
        Variables should always be enclosed in braces. If your tag contents are only on one line, you can enter it exactly as you wish. Be careful not to press enter at the end of the line.<br/>
        If the tag contents expand across multiple lines, the contents must be wrapped in single quotes. If a variable is used in a multiline tag contents, you must exclude it from the single quotes. See valid examples below.
    </p>
    <pre>my new value</pre>
    <pre>${cust_attr_1}</pre>
    <pre>My${cust_attr_1}value</pre>
    <pre>'
&lt;OOBE&gt;
&lt;HideEULAPage&gt;'${cust_attr_1}'&lt;/HideEULAPage&gt;
&lt;/OOBE&gt;
'</pre>
    <pre>'
&lt;OOBE&gt;
&lt;HideEULAPage&gt;true&lt;/HideEULAPage&gt;
&lt;/OOBE&gt;
'</pre>
    
    <h3>WinPE Imaging Environment Examples</h3>

    <p>Variables should always be enclosed in braces. Other than that, there is no need for any special formatting. See examples below.</p>
    <pre>my new value</pre>
    <pre>${cust_attr_1}</pre>
    <pre>My${cust_attr_1}value</pre>
    <pre>&lt;OOBE&gt;
&lt;HideEULAPage&gt;${cust_attr_1}&lt;/HideEULAPage&gt;
&lt;/OOBE&gt;</pre>
    <pre>&lt;OOBE&gt;
&lt;HideEULAPage&gt;true&lt;/HideEULAPage&gt;
&lt;/OOBE&gt;</pre>
</asp:Content>