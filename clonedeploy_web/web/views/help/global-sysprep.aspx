<%@ Page Title="" Language="C#" MasterPageFile="~/views/help/content.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="subcontent" Runat="Server">
     <script type="text/javascript">
         $(document).ready(function () {
             $('#global').addClass("nav-current");
             $('#global-sysprep').addClass("nav-current-sub");
         });
        </script>
    <h1>Global->Sysprep</h1>
   <p>Sysprep tags allow you make changes on the fly to a sysprep file during the imaging process.  You can use this to 
       replace almost any existing value in the sysprep answer file, you cannot add new entries.  To use this put in the 
       opening sysprep tag and the closing sysprep tag and the contents of what you want in between them.  
       Example:  Opening Tag: &lt;ComputerName&gt; Closing Tag &lt;/ComputerName&gt;.  Once defined the tags can then be assigned in the image profile.
       The contents can contain anything you want including variables such as custom attributes.  The contents can also contain other
       nested xml tags.  When using variables with Sysprep Tags or special character, everything other than the variable must be enclosed in
       single quotes.</p>
        <h3>Valid Contents Examples</h3>
    <p>my new value</p>
    <p>$cust_attr_1</p>
    <p>'My' $cust_attr_1 'value'</p>
    <p>'&lt;credentials&gt;
        <br />
        &lt;username&gt;'$cust_attr_1'&lt;/username&gt;
        <br />
         &lt;password&gt;'$cust_attr_2'&lt;/password&gt;
        <br/>
        &lt;/credentials&gt;'
    </p> 

</asp:Content>

